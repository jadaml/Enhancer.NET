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
using System.Linq.Expressions;

namespace Enhancer.Configuration
{
    /// <summary>
    /// Represents a setting value that is backed by an object's field or
    /// property.
    /// </summary>
    /// <typeparam name="T">
    /// The base type of the setting value.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// When committing changes on this setting value, the changes is applied
    /// back to the field or property that this instance is attached to.
    /// </para>
    /// <para>
    /// When initialized with the
    /// <see cref="AttachedValue{T}.AttachedValue(Expression{Func{T}})"/>
    /// constructor, the setting value will adapt to the new object. If the
    /// object; who's field or property this is attached to is a property,
    /// then the object will follow the value of the property rather than
    /// create a copy of the value and refer that.
    /// </para>
    /// </remarks>
    /// <revisionHistory>
    /// <revision date="2018-09-08" version="0.2.0">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public partial class AttachedValue<T> : ValueBase<T>
    {
        private Func<object> _refObj;
        private Func<T>      _getter;
        private Action<T>    _setter;

        /// <summary>
        /// Gets the object that is referred to when modifying the value.
        /// </summary>
        public object ReferenceObject => _refObj();

        /// <summary>
        /// Gets or sets the value of the backing field or property.
        /// </summary>
        public override T Reference
        {
            get => _getter();
            protected set
            {
                // Because IsModified only gets set to true when Value is set to
                // a different value, and Committing does not change Reference
                // when IsModified is false; all that in ValueBase<T>, the value
                // is guaranteed to be different.
                _setter(value);
                OnPropertyChanged();
            }
        }

        private AttachedValue() { }

        /// <summary>
        /// Creates a new <see cref="AttachedValue{T}"/> instance.
        /// </summary>
        /// <param name="referenceObject">
        /// The object who's field or property should be used for backing this
        /// setting value.
        /// </param>
        /// <param name="memberName">
        /// The name of the member to back this setting value.
        /// </param>
        public AttachedValue(object referenceObject, string memberName)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            MemberExpression    member    = Expression.PropertyOrField(Expression.Constant(referenceObject), memberName);

            _refObj = () => referenceObject;
            _getter = Expression.Lambda<Func<T>>(member).Compile();
            _setter = Expression.Lambda<Action<T>>(Expression.Assign(member, parameter), parameter).Compile();

            Rollback();
        }

        /// <summary>
        /// Creates a new <see cref="AttachedValue{T}"/> instance.
        /// </summary>
        /// <param name="member">
        /// An expression referencing the property to back this setting value.
        /// </param>
        public AttachedValue(Expression<Func<T>> member)
        {
            MemberExpression    ex    = member.Body as MemberExpression ?? throw new ArgumentException("", nameof(member));
            ParameterExpression param = Expression.Parameter(typeof(T));

            _refObj = Expression.Lambda<Func<object>>(ex.Expression).Compile();
            _getter = member.Compile();
            _setter = Expression.Lambda<Action<T>>(Expression.Assign(ex, param), param).Compile();

            Rollback();
        }

        /// <summary>
        /// Converts a value to an <see cref="AttachedValue{T}"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The <see cref="AttachedValue{T}"/> representation of the
        /// <paramref name="value"/>.
        /// </returns>
        /// <remarks>
        /// <note type="note">
        /// The resulting <see cref="AttachedValue{T}"/> is not a functioning
        /// value, and cannot be committed. The <see cref="Reference"/> property
        /// will throw an <see cref="InvalidOperationException"/>.
        /// </note>
        /// </remarks>
        public static implicit operator AttachedValue<T>(T value) => new TransitiveValue(value);
    }
}
