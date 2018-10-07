/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Configuration.
 *
 * Enhancer.Configuration is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Configuration is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Configuration.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a group of setting values.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    [DebuggerDisplay("Count = {Count}")]
    public class SettingsGroup : IList<IValue>, INotifyPropertyChanged
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private List<IValue> _values = new List<IValue>();

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        /// <value>The element at the specified index.</value>
        public IValue this[int index]
        {
            get => ((IList<IValue>)_values)[index];
            set => ((IList<IValue>)_values)[index] = value;
        }

        /// <summary>
        /// Gets the number of setting values contained in this <see cref="SettingsGroup"/>.
        /// </summary>
        public int Count => _values.Count;

        /// <summary>
        /// Gets a value indicating wether this collection can be modified.
        /// </summary>
        /// <value>
        /// This property always retrun <see langword="true"/>.
        /// </value>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets a value indicating whether there are any modifications in the
        /// <see cref="SettingsGroup"/>.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if there is a setting value that is modified,
        /// otherwise <see langword="false"/>.
        /// </value>
        public bool IsModified => _values.Any(v => (v as ITransactionedValue)?.IsModified ?? false);

        /// <summary>
        /// Raised when a property changed.
        /// </summary>
        /// <remarks>
        /// Only those properties raise this event which are related to the
        /// setting values, but not to the collection it self.
        /// </remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raised when the <see cref="SettingsGroup"/> is about to commit all
        /// the changes the contained setting values hold.
        /// </summary>
        public event EventHandler Committing;

        /// <summary>
        /// Raised when the <see cref="SettingsGroup"/> committed all the
        /// changes of the contained setting values.
        /// </summary>
        public event EventHandler Committed;

        /// <summary>
        /// Raised when the <see cref="SettingsGroup"/> is about to roll back
        /// all the changes made in the contained setting values.
        /// </summary>
        public event EventHandler AboutToRollback;

        /// <summary>
        /// Raised when all the setting values are rolled back contained in this
        /// <see cref="SettingsGroup"/>.
        /// </summary>
        public event EventHandler RolledBack;

        /// <summary>
        /// Adds all the IValue fields defined in the inherited class to the
        /// <see cref="SettingsGroup"/>.
        /// </summary>
        protected void InitializeValues()
        {
            IEnumerable<IValue> fields;

            fields = from field in GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                     where field.FieldType.GetInterface(nameof(IValue)) == typeof(IValue)
                     select (IValue)field.GetValue(this);

            _values.AddRange(fields);
        }

        /// <summary>
        /// Adds a setting value to the <see cref="SettingsGroup"/> to manage.
        /// </summary>
        /// <param name="item">
        /// The setting value to add.
        /// </param>
        public void Add(IValue item) => _values.Add(item);

        /// <summary>
        /// Removes all the setting values from the <see cref="SettingsGroup"/>.
        /// </summary>
        public void Clear() => _values.Clear();

        /// <summary>
        /// Checks if a setting value is element of this
        /// <see cref="SettingsGroup"/>.
        /// </summary>
        /// <param name="item">
        /// The setting value to check for.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the setting value is contained in this
        /// <see cref="SettingsGroup"/>, otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains(IValue item) => _values.Contains(item);

        /// <summary>
        /// Copies the elements of the <see cref="SettingsGroup"/> to a
        /// compatible one-dimensional array, starting at a particular index.
        /// </summary>
        /// <param name="array">
        /// The array to copy the elements to.
        /// </param>
        /// <param name="arrayIndex">
        /// The zero-based index at which the copying begins in the
        /// <paramref name="array"/>.
        /// </param>
        public void CopyTo(IValue[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IValue> GetEnumerator() => _values.GetEnumerator();

        /// <summary>
        /// Finds the index of the specified element in the collection.
        /// </summary>
        /// <param name="item">
        /// The element to look for.
        /// </param>
        /// <returns>
        /// The zero-based index of the element in the collection, if the
        /// element is contained in it, otherwise -1.
        /// </returns>
        public int IndexOf(IValue item) => _values.IndexOf(item);

        /// <summary>
        /// Inserts a new setting value at the specified index.
        /// </summary>
        /// <param name="index">
        /// The index to place in to the collection.
        /// </param>
        /// <param name="item">
        /// The element to add to the collection.
        /// </param>
        public void Insert(int index, IValue item) => _values.Insert(index, item);

        /// <summary>
        /// Removes the specified element from the collection, if it is
        /// contained by the collection.
        /// </summary>
        /// <param name="item">
        /// The element to remove from the collection.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the element was successfully removed from
        /// the collection, otherwise <see langword="false"/>.
        /// </returns>
        public bool Remove(IValue item) => _values.Remove(item);

        /// <summary>
        /// Removes an element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index to remove the element at.
        /// </param>
        public void RemoveAt(int index) => _values.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Finalizes all the changes on all the setting values in the
        /// <see cref="SettingsGroup"/> which supports it.
        /// </summary>
        /// <seealso cref="ITransactionedValue"/>
        /// <seealso cref="ITransactionedValue{T}"/>
        public void Commit()
        {
            OnCommitting(EventArgs.Empty);

            _values.ForEach(v => (v as ITransactionedValue)?.Commit());

            OnCommitted(EventArgs.Empty);
        }

        /// <summary>
        /// Reverts all the changes in all the setting values in the
        /// <see cref="SettingsGroup"/> which supports it.
        /// </summary>
        /// <seealso cref="ITransactionedValue"/>
        /// <seealso cref="ITransactionedValue{T}"/>
        public void Rollback()
        {
            OnAboutToRollback(EventArgs.Empty);

            _values.ForEach(v => (v as ITransactionedValue)?.Rollback());

            OnRolledBack(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref="Committing"/> event.
        /// </summary>
        /// <param name="e">
        /// Additional arguments for the event.
        /// </param>
        protected virtual void OnCommitting(EventArgs e) => Committing?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="Committed"/> event.
        /// </summary>
        /// <param name="e">
        /// Additional arguments for the event.
        /// </param>
        protected virtual void OnCommitted(EventArgs e) => Committed?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="AboutToRollback"/> event.
        /// </summary>
        /// <param name="e">
        /// Additional arguments for the event.
        /// </param>
        protected virtual void OnAboutToRollback(EventArgs e) => AboutToRollback?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="RolledBack"/> event.
        /// </summary>
        /// <param name="e">
        /// Additional arguments for the event.
        /// </param>
        protected virtual void OnRolledBack(EventArgs e) => RolledBack?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that was changed.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
