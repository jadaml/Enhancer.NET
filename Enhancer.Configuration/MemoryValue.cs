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

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a setting value who's reference value is stored in memory.
    /// </summary>
    /// <typeparam name="T">
    /// The base type of the setting value.
    /// </typeparam>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public partial class MemoryValue<T> : ValueBase<T>
    {
        private T _reference;

        /// <inheritdoc/>
        public override T Reference
        {
            get => _reference;
            protected set
            {
                // Because IsModified only gets set to true when Value is set to
                // a different value, and Committing does not change Reference
                // when IsModified is false; all that in ValueBase<T>, the value
                // is guaranteed to be different.
                _reference = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Creates a new <see cref="MemoryValue{T}"/> instance with the default
        /// value of <typeparamref name="T"/> as it's initial value.
        /// </summary>
        public MemoryValue() : this(default(T)) { }

        /// <summary>
        /// Creates a new <see cref="MemoryValue{T}"/> instance with a specified
        /// initial value.
        /// </summary>
        /// <param name="initialValue">
        /// The initial value of the setting value.
        /// </param>
        public MemoryValue(T initialValue)
        {
            _reference = initialValue;
        }

        /// <summary>
        /// Casts a <typeparamref name="T"/> to a <see cref="MemoryValue{T}"/>
        /// value.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>
        /// The <see cref="MemoryValue{T}"/> representation of the
        /// <paramref name="value"/>.
        /// </returns>
        public static implicit operator MemoryValue<T>(T value) => new MemoryValue<T>(value);
    }
}
