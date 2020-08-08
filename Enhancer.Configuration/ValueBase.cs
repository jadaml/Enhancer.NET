/* Copyright (c) 2018, 2020, Ádám L. Juhász
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Enhancer.Configuration
{
    /// <summary>
    /// This is the base class of a transaction based setting value.
    /// </summary>
    /// <typeparam name="T">The base type of the setting value.</typeparam>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public abstract partial class ValueBase<T>
        : ITransactionedValue, ITransactionedValue<T>, IValueChanged, INotifyPropertyChanged, IEquatable<T>, IFormattable
    {
        private T                    _value;
        private bool                 _isModified = false;
        private bool                 _autoCommit;
        private IEqualityComparer<T> _comparer;

        /// <summary>
        /// Gets or sets the <see cref="IEqualityComparer{T}"/> to compare the
        /// current <see cref="Value"/> with the <see cref="Reference"/> value.
        /// This comparer will also be used by the <see cref="Equals(T)"/> and
        /// <see cref="GetHashCode"/> methods.
        /// </summary>
        public virtual IEqualityComparer<T> Comparer
        {
            get => _comparer ?? (_comparer = EqualityComparer<T>.Default);
            set => _comparer = value;
        }

        /// <summary>
        /// Gets or sets the value of this setting value object.
        /// </summary>
        public virtual T Value
        {
            get => IsModified ? _value : Reference;
            set
            {
                if (IsModified && Comparer.Equals(_value, value)
                 || !IsModified && Comparer.Equals(Reference, value))
                {
                    return;
                }

                _value     = value;
                IsModified = !Comparer.Equals(Reference, value);

                OnValueChanged(EventArgs.Empty);
                OnPropertyChanged();

                if (AutoCommit && IsModified)
                {
                    Commit();
                }
            }
        }

        /// <summary>
        /// Gets or sets the reference value for this setting value.
        /// </summary>
        public abstract T Reference { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the setting value got modified.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the value got modified since the changes
        /// last finalized, otherwise <see langword="false"/>.
        /// </value>
        public bool IsModified
        {
            get => _isModified;
            private set
            {
                if (_isModified == value) return;
                _isModified = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the changes to
        /// <see cref="Value"/> are immediately gets finalized or not.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the changes gets immediately finalized,
        /// otherwise <see langword="false"/>.
        /// </value>
        public virtual bool AutoCommit
        {
            get => _autoCommit;
            set
            {
                if (_autoCommit == value) return;

                _autoCommit = value;

                OnPropertyChanged();
            }
        }

        object ITransactionedValue.Reference => Reference;

        object IValue.Value
        {
            get => Value;
            set => Value = (T)value;
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format("{0}{1}{2}{3}{2}{4}",
                    null,
                    false ? ": " : "",
                    Value is string ? "\"" : "",
                    Value,
                    IsModified ? " (Modified)" : "");
            }
        }

        /// <summary>
        /// Raised when the <see cref="Value"/> gets changed.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Raised when the modifications are finalized.
        /// </summary>
        public event EventHandler Committed;

        /// <summary>
        /// Raised when the modifications are reverted.
        /// </summary>
        public event EventHandler RolledBack;

        /// <summary>
        /// Raised whenever one or all properties got changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Finalizes the changes of <see cref="Value"/>.
        /// </summary>
        public virtual void Commit()
        {
            if (!IsModified)
            {
                OnCommitted(EventArgs.Empty);
                return;
            }

            Reference  = Value;
            IsModified = false;
            OnCommitted(EventArgs.Empty);
        }

        /// <summary>
        /// Reverts the changes of <see cref="Value"/>.
        /// </summary>
        public virtual void Rollback()
        {
            if (!IsModified)
            {
                OnRolledBack(EventArgs.Empty);
                return;
            }

            Value      = Reference;
            IsModified = false;
            OnRolledBack(EventArgs.Empty);
        }

        /// <summary>
        /// Raises the <see cref                           ="ValueChanged"/> event.
        /// </summary>
        /// <param name                                    ="e">Arguments for the event.</param>
        protected virtual void OnValueChanged(EventArgs e) => ValueChanged?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="Committed"/> event.
        /// </summary>
        /// <param name="e">Arguments for the event.</param>
        protected virtual void OnCommitted(EventArgs e) => Committed?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="RolledBack"/> event.
        /// </summary>
        /// <param name="e">The arguments for the event.</param>
        protected virtual void OnRolledBack(EventArgs e) => RolledBack?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that was changed, or <see langword="null"/>
        /// if all property was changed.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Converts the object to it's string representation.
        /// </summary>
        /// <returns>The string representation of the object.</returns>
        public override string ToString() => ToString(null, null);

        /// <summary>
        /// Converts the object to it's string representation.
        /// </summary>
        /// <param name="format">
        /// The format to use when converting the object.
        /// </param>
        /// <returns>The string representation of the object.</returns>
        /// <remarks>
        /// This method formats the setting value as the
        /// <typeparamref name="T"/> base type would, if it implements
        /// <see cref="IFormattable"/>.
        /// It will call <see cref="object.ToString()"/> if the type doesn't
        /// implements <see cref="IFormattable"/>.
        /// </remarks>
        public string ToString(string format) => ToString(format, null);

        /// <summary>
        /// Converts the object to it's string representation.
        /// </summary>
        /// <param name="format">
        /// The format to use when converting the object.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider to format the object with.
        /// </param>
        /// <returns>The string representation of the object.</returns>
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

            if (Value == null)
            {
                return string.Empty;
            }

            if (Value is IFormattable formattable)
            {
                return formattable.ToString(format, formatProvider);
            }

            return Value.ToString();
        }

        /// <summary>
        /// Checks if the provided object is equal to the current object.
        /// </summary>
        /// <param name="obj">
        /// The provided object to compare the object with.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the object equals with the provided
        /// object, otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is ValueBase<T> && ReferenceEquals(obj, this)
                || obj is T && Equals((T)obj)
                || obj == null && _value == null;
        }

        /// <summary>
        /// Checks if the provided object is equal to the current object.
        /// </summary>
        /// <param name="other">
        /// The provided object to compare the object with.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the object equals with the provided
        /// object, otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(T other) => Comparer.Equals(_value, other);

        /// <summary>
        /// Generates a hash code for the object.
        /// </summary>
        /// <returns>The generated hash code for the object.</returns>
        /// <remarks>
        /// <note type="important">
        /// The hash code may change when the <see cref="Value"/> property
        /// changed.
        /// </note>
        /// </remarks>
        public override int GetHashCode() => _value == null ? 0 : Comparer.GetHashCode(_value);

        /// <summary>
        /// Casts a <see cref="ValueBase{T}"/> to a
        /// <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <returns>The <typeparamref name="T"/> representation of the value.</returns>
        public static implicit operator T(ValueBase<T> value) => value.Value;

        /// <summary>
        /// Casts the <typeparamref name="T"/> value to a
        /// <see cref="ValueBase{T}"/> value.
        /// </summary>
        /// <param name="value">The value to cast.</param>
        /// <remarks>
        /// <note type="important">
        /// The resulting <see cref="ValueBase{T}"/> is not a functioning
        /// value, and cannot be committed. The <see cref="Reference"/> property
        /// will throw an <see cref="InvalidOperationException"/>.
        /// </note>
        /// </remarks>
        /// <returns>
        /// The <see cref="ValueBase{T}"/> representation of the value.
        /// </returns>
        public static implicit operator ValueBase<T>(T value) => new TransitiveValue(value);
    }
}
