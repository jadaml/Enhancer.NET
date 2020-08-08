/* Copyright (c) 2019, 2020, Ádám L. Juhász
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

namespace Enhancer.Extensions
{
    /// <summary>
    /// Marks a property relying on another within the same type.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <revisionHistory>
    /// <revision date="2019-03-31" version="0.2.0">
    /// Introduced.
    /// </revision>
    /// </revisionHistory>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class DependsOnPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the property the associated property is dependent on.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DependsOnPropertyAttribute"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property the associated property depends on.</param>
        public DependsOnPropertyAttribute(string propertyName) => PropertyName = propertyName;
    }
}
