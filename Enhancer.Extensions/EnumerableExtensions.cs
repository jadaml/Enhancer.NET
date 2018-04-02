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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool IsAnyOf<T>(this T value, params T[] args)
        {
            return IsAnyOf(value, args.AsEnumerable());
        }

        public static bool IsAnyOf<T>(this T value, IEnumerable<T> collection)
        {
            return collection.Contains(value);
        }

        public static bool IsNeitherOf<T>(this T value, params T[] args)
        {
            return IsNeitherOf(value, args.AsEnumerable());
        }

        public static bool IsNeitherOf<T>(this T value, IEnumerable<T> collection)
        {
            return !collection.Contains(value);
        }

        public static bool HaveAtLeast(this IEnumerable collection, int count)
        {
            int i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i < count && enumerator.MoveNext(); ++i)
            { }

            return i >= count;
        }

        public static bool HaveAtMost(this IEnumerable collection, int count)
        {
            int i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i <= count && enumerator.MoveNext(); ++i)
            { }

            return i <= count;
        }

        public static bool HaveExactly(this IEnumerable collection, int count)
        {
            int i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i <= count && enumerator.MoveNext(); ++i)
            { }

            return i == count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(this IList<T> list, Random random, int offset = 0)
        {
            return Select(random, list, offset, list.Count - offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(this IList<T> list, Random random, int offset, int count)
        {
            return Select(random, list, offset, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Select<T>(this Random random, IList<T> list, int offset = 0)
        {
            return Select(random, list, offset, list.Count - offset);
        }

        public static T Select<T>(this Random random, IList<T> list, int offset, int count)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (offset + count >= list.Count)
            {
                throw new ArgumentException("The specified arguments exceeds the list.");
            }

            return list[random.Next(offset, count + offset)];
        }

        public static void Add(this IList list, object value, int amount)
        {
            if (list.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add values to the collection, because it is read-only.");
            }

            for (int c = 0; c < amount; ++c)
            {
                list.Add(value);
            }
        }

        public static void AddMany(this IList list, Type type, int amount, params object[] args) => AddMany(list, type, args.Select(arg => arg.GetType()).ToArray(), amount, args);

        public static void AddMany(this IList list, Type type, Type[] signature, int amount, params object[] args)
        {
            ConstructorInfo ctor;

            if (list.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add values to the collection, because it is read-only.");
            }

            if ((ctor = type.GetConstructor(signature)) is null)
            {
                throw new ArgumentException($"The specified {type} type does not have a public constructor with {signature.Length} number of arguments.");
            }

            for (int c = 0; c < amount; c++)
            {
                list.Add(ctor.Invoke(args));
            }
        }

        public static void Add(this IList list, Func<object> function, int amount)
        {
            if (list.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add values to the collection, because it is read-only.");
            }

            for (int c = 0; c < amount; c++)
            {
                list.Add(function.Invoke());
            }
        }

        /// <summary>
        /// Adds many amount of the same function to the collection.
        /// </summary>
        /// <typeparam name="TSource">The base type of the collection</typeparam>
        /// <param name="collection">The collection to add to
        /// (You can also call this function as if would be defined in this object.)</param>
        /// <param name="function">The object to add to the collection</param>
        /// <param name="amount">The amount of times to add the object to the collection</param>
        public static void Add<TSource>(this ICollection<TSource> collection, TSource value, int amount)
        {
            if (collection.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add values to the collection, because it is read-only.");
            }

            for (int c = 0; c < amount; c++)
            {
                collection.Add(value);
            }
        }

        public static void Add<TSource>(this ICollection<TSource> collection, Func<TSource> function, int amount)
        {
            if (collection.IsReadOnly)
            {
                throw new InvalidOperationException("Cannot add values to the collection, because it is read-only.");
            }

            for (int c = 0; c < amount; c++)
            {
                collection.Add(function.Invoke());
            }
        }
    }
}
