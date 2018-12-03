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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a simple settings value that is not transaction based and is
    /// stored in memory.
    /// </summary>
    /// <typeparam name="T">The base type of the value.</typeparam>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    [DebuggerDisplay("{Value}")]
    public partial class SimpleValue<T> : IValue, IValue<T>, IValueChanged, INotifyPropertyChanged, IEquatable<T>, IFormattable
    {
        private T _value;

        /// <summary>
        /// Gets or sets the value of the settings value.
        /// </summary>
        public virtual T Value
        {
            get => _value;
            set
            {
                if (_value?.Equals(value) ?? value == null) return;

                _value = value;

                OnValueChanged(EventArgs.Empty);
                OnPropertyChanged();
            }
        }

        object IValue.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        /// <summary>
        /// Raised when <see cref="Value"/> got changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Raised when one or all of the object's property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Creates a new instance of <see cref="SimpleValue{T}"/> instance
        /// which is initialized with the default value of
        /// <typeparamref name="T"/>.
        /// </summary>
        public SimpleValue() : this(default(T)) { }

        /// <summary>
        /// Creates a new <see cref="SimpleValue{T}"/> instance initialized with
        /// the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The initial value of the setting.</param>
        public SimpleValue(T value) => _value = value;

        /// <summary>
        /// Raises the <see cref="ValueChanged"/> event.
        /// </summary>
        /// <param name="e">Arguments to pass to the event.</param>
        protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that was changed, or
        /// <see langword="null"/> if all properties changed.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <returns>The string representation of this object.</returns>
        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <param name="format">The format specifier to format this object with.</param>
        /// <returns>The string representation of this object.</returns>
        /// <remarks>
        /// This method formats the setting value as the
        /// <typeparamref name="T"/> base type would, if it implements
        /// <see cref="IFormattable"/>.
        /// It will call <see cref="object.ToString()"/> if the type doesn't
        /// implements <see cref="IFormattable"/>.
        /// </remarks>
        public string ToString(string format) => ToString(format, null);

        /// <summary>
        /// Gets the string representation of this object.
        /// </summary>
        /// <param name="format">The format specifier to format this object with.</param>
        /// <param name="formatProvider">A format provider to format the object with.</param>
        /// <returns>The string representation of this object.</returns>
        /// <remarks>
        /// This method formats the setting value as the
        /// <typeparamref name="T"/> base type would, if it implements
        /// <see cref="IFormattable"/>.
        /// It will call <see cref="object.ToString()"/> if the type doesn't
        /// implements <see cref="IFormattable"/>.
        /// </remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            // return (_value as IFormattable)?.ToString(format, formatProvider) ?? _value?.ToString() ?? "";
            // The above line; although beautiful in it's brevity;
            // `_value?.ToString()` generates a condition meaningless and
            // untestable in our case (see related decompiled unittest),
            // so ***STICK WITH THIS CODE!***

            if (_value == null)
            {
                return string.Empty;
            }

            if (_value is IFormattable formattable)
            {
                return formattable.ToString(format, formatProvider);
            }

            return _value.ToString();
        }

        /// <summary>
        /// Checks if the object is equal to another.
        /// </summary>
        /// <param name="obj">
        /// The other object to compare this object with.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this object is equal to the specified
        /// object, otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj) => obj is T && Equals((T)obj) || obj == null && _value == null;

        /// <summary>
        /// Checks if the object is equal to another.
        /// </summary>
        /// <param name="other">
        /// The other object to compare this object with.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this object is equal to the specified
        /// object, otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(T other) => _value.Equals(other);

        /// <summary>
        /// Calculates a hash code for the object.
        /// </summary>
        /// <returns>The hash code for the object.</returns>
        /// <remarks>
        /// <note type="important">
        /// The hash code may change when the <see cref="Value"/> property
        /// changed.
        /// </note>
        /// </remarks>
        public override int GetHashCode()
        {
            if (_value == null)
            {
                return 0;
            }

            return _value.GetHashCode();
        }

        /// <summary>
        /// Casts a <see cref="SimpleValue{T}"/> to a <typeparamref name="T"/>
        /// value.
        /// </summary>
        /// <param name="value">
        /// The <typeparamref name="T"/> value representation of the
        /// <see cref="SimpleValue{T}"/> object.
        /// </param>
        /// <returns>
        /// The <typeparamref name="T"/> representation of the
        /// <paramref name="value"/>.
        /// </returns>
        public static implicit operator T(SimpleValue<T> value) => value._value;

        /// <summary>
        /// Casts a <typeparamref name="T"/> value to a
        /// <see cref="SimpleValue{T}"/> setting value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="SimpleValue{T}"/> representation of the given
        /// <typeparamref name="T"/> value.
        /// </param>
        /// <returns>
        /// The <see cref="SimpleValue{T}"/> representation of the
        /// <paramref name="value"/>.
        /// </returns>
        public static implicit operator SimpleValue<T>(T value) => new SimpleValue<T>(value);
    }
}
