/* Copyright (c) 2018, Ádám L. Juhász
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
using System.Linq.Expressions;
using static System.Linq.Expressions.Expression;
using static System.Enum;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class provides extension methods for enumeration types.
    /// </summary>
    /// <revisionHistory>
    /// <revision date="2018-09-22" version="0.2.0-0.1" author="jadaml">
    /// First introduced.
    /// </revision>
    /// </revisionHistory>
    public static class EnumExtensions
    {
        /// <summary>
        /// Helper class for optimized bit-wise operators on enumeration values.
        /// </summary>
        /// <typeparam name="T">The type of the enumeration to work with.</typeparam>
        private static class EnumOperators<T> where T : Enum
        {
            public static readonly Func<T, T, T> Or;
            public static readonly Func<T, T, T> And;
            public static readonly Func<T, T>    Not;

            static EnumOperators()
            {
                ParameterExpression leftParameter, rightParameter;

                leftParameter  = Parameter(typeof(T));
                rightParameter = Parameter(typeof(T));
                
                Or  = Lambda<Func<T, T, T>>(Convert(Expression.Or(Convert(leftParameter, GetUnderlyingType(typeof(T))), Convert(rightParameter, GetUnderlyingType(typeof(T)))), typeof(T)), leftParameter, rightParameter).Compile();
                And = Lambda<Func<T, T, T>>(Convert(Expression.And(Convert(leftParameter, GetUnderlyingType(typeof(T))), Convert(rightParameter, GetUnderlyingType(typeof(T)))), typeof(T)), leftParameter, rightParameter).Compile();
                Not = Lambda<Func<T, T>>(Convert(Expression.Not(Convert(leftParameter, GetUnderlyingType(typeof(T)))), typeof(T)), leftParameter).Compile();
            }
        }

        /// <summary>
        /// Toggles the state of a specified <paramref name="flag"/> of a flag
        /// enumeration in the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of the flag enumeration.</typeparam>
        /// <param name="value">The value to work with.</param>
        /// <param name="flag">The flag value to toggle.</param>
        /// <returns>
        /// The new value where the specified <paramref name="flag"/> is set to
        /// it's negated state.
        /// </returns>
        /// <revisionHistory>
        /// <revision date="2018-09-22" version="0.2.0-0.1" author="jadaml">
        /// First introduced.
        /// </revision>
        /// </revisionHistory>
        public static T ToggleFlag<T>(this T value, T flag) where T : Enum
        {
            return ToggleFlag(value, flag, !value.HasFlag(flag));
        }

        /// <summary>
        /// Switches the state of a specified <paramref name="flag"/> of a flag
        /// enumeration in the given <paramref name="value"/> to the specified
        /// <paramref name="state"/>.
        /// </summary>
        /// <typeparam name="T">The type of the flag enumeration.</typeparam>
        /// <param name="value">The value to work with.</param>
        /// <param name="flag">The flag value to toggle.</param>
        /// <param name="state">
        /// <see langword="true"/> if the flag should be present, otherwise
        /// <see langword="false"/>.
        /// </param>
        /// <returns>
        /// The new value where the specified <paramref name="flag"/> is set to
        /// the specified <paramref name="state"/>.
        /// </returns>
        /// <revisionHistory>
        /// <revision date="2018-09-22" version="0.2.0-0.1" author="jadaml">
        /// First introduced.
        /// </revision>
        /// </revisionHistory>
        public static T ToggleFlag<T>(this T value, T flag, bool state) where T : Enum
        {
            if (state)
            {
                return EnumOperators<T>.Or(value, flag); // value | flag
            }
            else
            {
                return EnumOperators<T>.And(value, EnumOperators<T>.Not(flag)); // value & ~flag
            }
        }
    }
}
