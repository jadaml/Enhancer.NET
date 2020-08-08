/* Copyright (c) 2019, Ádám L. Juhász
 *
 * This file is part of Enhancer.Extensions.
 *
 * Enhancer.Extensions is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Extensions is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Extensions.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PropertyDependantsMap = System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IList<string>>;
using TypePropertyDependencyMap = System.Collections.Generic.IDictionary<System.Type, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IList<string>>>;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class provides methods to raise the
    /// <see cref="System.ComponentModel.INotifyPropertyChanging.PropertyChanging"/> and
    /// <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> events
    /// for properties which are refer to a property with the <see cref="DependsOnPropertyAttribute"/>.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanging"/>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged"/>
    /// <seealso cref="IRaiseNotifyPropertyChanging"/>
    /// <seealso cref="IRaiseNotifyPropertyChanged"/>
    /// <seealso cref="DependsOnPropertyAttribute"/>
    /// <revisionHistory>
    /// <revision date="2019-03-31" version="0.2.0" author="Ádám L. Juhász &lt;jadaml@gmail.com&gt;">
    /// Introduced.
    /// </revision>
    /// </revisionHistory>
    public static class PropertyNotifier
    {
        private static readonly TypePropertyDependencyMap                            _dependantProperties;
        private static readonly HashSet<Tuple<IRaiseNotifyPropertyChanging, string>> _changingHistory  = new HashSet<Tuple<IRaiseNotifyPropertyChanging, string>>();
        private static readonly HashSet<Tuple<IRaiseNotifyPropertyChanged, string>>  _changedHistory   = new HashSet<Tuple<IRaiseNotifyPropertyChanged, string>>();
        private static readonly object                                               _changingHistLock = new object();
        private static readonly object                                               _changedHistLock  = new object();

        static PropertyNotifier()
        {
            // Much more neater here.
            _dependantProperties = new Dictionary<Type, PropertyDependantsMap>();
        }

        /// <summary>
        /// Raises the <see cref="System.ComponentModel.INotifyPropertyChanging.PropertyChanging"/> event for
        /// all properties of the sender depending on the specified
        /// <paramref name="propertyName"/> of the <paramref name="sender"/>.
        /// </summary>
        /// <param name="sender">
        /// The sender who's property about to change and should notify it's dependent properties changig.
        /// </param>
        /// <param name="propertyName">Name of the changing property.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="sender"/> or <paramref name="propertyName"/> is <see langword="null"/>
        /// </exception>
        public static void RaiseDependentPropertyChanging(this IRaiseNotifyPropertyChanging sender, string propertyName)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            lock (_changingHistLock)
            {
                if (_changingHistory.Contains(Tuple.Create(sender, propertyName)))
                {
                    return;
                }

                _changingHistory.Add(Tuple.Create(sender, propertyName));
            }

            try
            {
                if (!GetDependantsMap(sender.GetType()).TryGetValue(propertyName, out IList<string> dependents))
                {
                    return;
                }

                foreach (string dependent in dependents)
                {
                    lock (_changingHistLock)
                    {
                        if (_changingHistory.Contains(Tuple.Create(sender, dependent)))
                        {
                            continue;
                        }
                    }

                    sender.OnPropertyChanging(dependent);
                }
            }
            finally
            {
                lock (_changingHistLock)
                {
                    _changingHistory.Remove(Tuple.Create(sender, propertyName));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"/> event for
        /// all properties of the sender depending on the specified <paramref name="propertyName"/> of the
        /// <paramref name="sender"/>.
        /// </summary>
        /// <param name="sender">
        /// The sender who's property changed and should notify it's dependent properties changed.
        /// </param>
        /// <param name="propertyName">The name of the changed property.</param>
        /// <exception cref="ArgumentNullException">
        /// Either <paramref name="sender"/> or <paramref name="propertyName"/> is <see langword="null"/>.
        /// </exception>
        public static void RaiseDependentPropertyChanged(this IRaiseNotifyPropertyChanged sender, string propertyName)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            lock (_changedHistLock)
            {
                if (_changedHistory.Contains(Tuple.Create(sender, propertyName)))
                {
                    return;
                }

                _changedHistory.Add(Tuple.Create(sender, propertyName));
            }

            try
            {
                if (!GetDependantsMap(sender.GetType()).TryGetValue(propertyName, out IList<string> dependents))
                {
                    return;
                }

                foreach (string dependent in dependents)
                {
                    lock (_changedHistLock)
                    {
                        if (_changedHistory.Contains(Tuple.Create(sender, dependent)))
                        {
                            continue;
                        }
                    }

                    sender.OnPropertyChanged(dependent);
                }
            }
            finally
            {
                lock (_changedHistLock)
                {
                    _changedHistory.Remove(Tuple.Create(sender, propertyName));
                }
            }
        }

        private static PropertyDependantsMap GetDependantsMap(Type type)
        {
            if (_dependantProperties.TryGetValue(type, out PropertyDependantsMap result))
            {
                return result;
            }

            Dictionary<string, IList<string>> dependantsMap = new Dictionary<string, IList<string>>(StringComparer.Ordinal);

            foreach (var deps in from pinfo in type.GetProperties()
                                    let dependencies = pinfo.GetCustomAttributes<DependsOnPropertyAttribute>()
                                    select new { Dependant = pinfo.Name, Dependencies = dependencies.Select(attrib => attrib.PropertyName) })
            {
                foreach (string dependency in deps.Dependencies.Where(pn => pn != null))
                {
                    if (dependantsMap.ContainsKey(dependency))
                    {
                        dependantsMap[dependency].Add(deps.Dependant);
                    }
                    else
                    {
                        dependantsMap.Add(dependency, new List<string>(new[] { deps.Dependant }));
                    }
                }
            }

            foreach (string key in dependantsMap.Keys.ToArray())
            {
                dependantsMap[key] = new List<string>(dependantsMap[key]);
            }

            _dependantProperties.Add(type, dependantsMap);

            return dependantsMap;
        }
    }
}
