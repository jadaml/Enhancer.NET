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
using System.Linq;
using static Enhancer.Extensions.Properties.Resources;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class holds extension methods for <see cref="Type"/> instances.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets the generic type that is implemented/inherited by the specified type based on the generic type definition.
        /// </summary>
        /// <typeparam name="T">The type of the value to check.</typeparam>
        /// <param name="type">The object who's type should be checked.</param>
        /// <param name="genericType">The generic type to find.</param>
        /// <returns>
        /// The type who's generic type definition matches with the specified one, otherwise <see langword="null"/>
        /// if the generic type definition was not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="type"/> or <paramref name="genericType"/>
        /// is <see langword="null"/>.
        /// </exception>
        public static Type GetGenericType<T>(this T type, Type genericType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return GetGenericType(typeof(T), genericType);
        }

        /// <summary>
        /// Gets the generic type that is implemented/inherited by the specified type based on the generic type definition.
        /// </summary>
        /// <param name="type">The type that should be checked.</param>
        /// <param name="genericType">The generic type to find.</param>
        /// <returns>
        /// The type who's generic type definition matches with the specified one, otherwise <see langword="null"/>
        /// if the generic type definition was not found.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="type"/> or <paramref name="genericType"/>
        /// is <see langword="null"/>.
        /// </exception>
        public static Type GetGenericType(this Type type, Type genericType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }

            if (genericType.IsInterface)
            {
                return type.GetInterface(genericType.Name);
            }

            if (!genericType.IsGenericTypeDefinition && (type == genericType || type.IsSubclassOf(genericType)))
            {
                return genericType;
            }
            else if (!genericType.IsGenericTypeDefinition)
            {
                return null;
            }

            for (; type != typeof(object); type = type.BaseType)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                {
                    return type;
                }
            }

            return null;
        }
    }
}
