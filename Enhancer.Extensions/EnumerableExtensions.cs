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
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using static Enhancer.Extensions.Properties.Resources;

namespace Enhancer.Extensions
{
    /// <summary>
    /// This class provides additional extension methods related to collections.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check if a given value is contained in a collection
        /// </summary>
        /// <typeparam name="T">The type of the value to check for.</typeparam>
        /// <param name="value">The value to check for.</param>
        /// <param name="args">The collection in which the value should be searched for.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> can be find in the specified collection,
        /// otherwise <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyOf<T>(this T value, params T[] args) => IsAnyOf(value, args.AsEnumerable());

        /// <summary>
        /// Check if a given value is contained in a collection
        /// </summary>
        /// <typeparam name="T">The type of the value to check for.</typeparam>
        /// <param name="value">The value to check for.</param>
        /// <param name="collection">The collection in which the value should be searched for.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> can be find in the specified collection,
        /// otherwise <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAnyOf<T>(this T value, IEnumerable<T> collection) => collection.Contains(value);

        /// <summary>
        /// Checks if a value is not part of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the value to look for.</typeparam>
        /// <param name="value">The value to look for.</param>
        /// <param name="args">The collection that should not contain the value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> could not be found in the specified collection,
        /// otherwise <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNeitherOf<T>(this T value, params T[] args) => IsNeitherOf(value, args.AsEnumerable());

        /// <summary>
        /// Checks if a value is not part of the specified collection.
        /// </summary>
        /// <typeparam name="T">The type of the value to look for.</typeparam>
        /// <param name="value">The value to look for.</param>
        /// <param name="collection">The collection that should not contain the value.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> could not be found in the specified collection,
        /// otherwise <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNeitherOf<T>(this T value, IEnumerable<T> collection) => !collection.Contains(value);

        /// <summary>
        /// Checks if the collection have at least <paramref name="count"/> number of elements.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="count">The number of elements the collection should have.</param>
        /// <returns>
        /// <see langword="true"/> if the collection have at least <paramref name="count"/> number of elements,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool HaveAtLeast(this IEnumerable collection, int count)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is Array)
            {
                return ((Array)collection).Length >= count;
            }

            if (collection is ICollection)
            {
                return ((ICollection)collection).Count >= count;
            }

            Type type;

            if ((type = collection.GetType().GetInterface(typeof(ICollection<>).Name)) != null)
            {
                return (int)type.GetProperty(nameof(ICollection<object>.Count)).GetValue(collection) >= count;
            }

            int         i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i < count && enumerator.MoveNext(); ++i) ;

            return i == count;
        }

        /// <summary>
        /// Checks if the collection does not have more then <paramref name="count"/> number of elements.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="count">The number of elements the collection should have.</param>
        /// <returns>
        /// <see langword="true"/> if the collection have at most <paramref name="count"/> number of elements,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool HaveAtMost(this IEnumerable collection, int count)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is Array)
            {
                return ((Array)collection).Length <= count;
            }

            if (collection is ICollection)
            {
                return ((ICollection)collection).Count <= count;
            }

            Type type;

            if ((type = collection.GetType().GetInterface(typeof(ICollection<>).Name)) != null)
            {
                return (int)type.GetProperty(nameof(ICollection<object>.Count)).GetValue(collection) <= count;
            }

            int         i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i <= count && enumerator.MoveNext(); ++i) ;

            return i <= count;
        }

        /// <summary>
        /// Checks if the collection have exactly <paramref name="count"/> number of elements.
        /// </summary>
        /// <param name="collection">The collection to check.</param>
        /// <param name="count">The number of elements the collection should have.</param>
        /// <returns>
        /// <see langword="true"/> if the collection have exactly <paramref name="count"/> number of elements,
        /// otherwise <see langword="false"/>.
        /// </returns>
        public static bool HaveExactly(this IEnumerable collection, int count)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is Array)
            {
                return ((Array)collection).Length == count;
            }

            if (collection is ICollection)
            {
                return ((ICollection)collection).Count == count;
            }

            Type type;

            if ((type = collection.GetType().GetInterface(typeof(ICollection<>).Name)) != null)
            {
                return (int)type.GetProperty(nameof(ICollection<object>.Count)).GetValue(collection) == count;
            }

            int i;
            IEnumerator enumerator;

            for (i = 0, enumerator = collection.GetEnumerator(); i <= count && enumerator.MoveNext(); ++i) ;

            return i == count;
        }

        /// <summary>
        /// Select a random element from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to select from.</param>
        /// <param name="random">The random object to select with.</param>
        /// <param name="offset">The lower bound to start the element selection from.</param>
        /// <returns>The element selected by the <paramref name="random"/> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(this IList<T> list, Random random, int offset = 0) => Select(random, list, offset, (list?.Count ?? 0) - offset);

        /// <summary>
        /// Select a random element from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to select from.</param>
        /// <param name="random">The random object to select with.</param>
        /// <param name="offset">The lower bound to start the element selection from.</param>
        /// <param name="count">
        /// The number of consecutive element that can be selected starting from <paramref name="offset"/>.
        /// </param>
        /// <returns>The element selected by the <paramref name="random"/> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T RandomElement<T>(this IList<T> list, Random random, int offset, int count) => Select(random, list, offset, count);

        /// <summary>
        /// Select a random element from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to select from.</param>
        /// <param name="random">The random object to select with.</param>
        /// <param name="offset">The lower bound to start the element selection from.</param>
        /// <returns>The element selected by the <paramref name="random"/> object.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Select<T>(this Random random, IList<T> list, int offset = 0) => Select(random, list, offset, (list?.Count ?? 0) - offset);

        /// <summary>
        /// Select a random element from a list.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to select from.</param>
        /// <param name="random">The random object to select with.</param>
        /// <param name="offset">The lower bound to start the element selection from.</param>
        /// <param name="count">
        /// The number of consecutive element that can be selected starting from <paramref name="offset"/>.
        /// </param>
        /// <returns>The element selected by the <paramref name="random"/> object.</returns>
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

            if (offset + count > list.Count)
            {
                throw new ArgumentException(ArgsOutOfListRange);
            }

            return list[random.Next(offset, count + offset)];
        }

        /// <summary>
        /// Adds a value by the specified amount to the collection.
        /// </summary>
        /// <param name="list">The collection to add to.</param>
        /// <param name="value">The value to add to the collection many times.</param>
        /// <param name="amount">The amount of times to call the function and add the result to the collection</param>
        public static void Add(this IList list, object value, int amount)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; ++c)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Adds many object instance to the collection by the specified amount, invoking the same constructor
        /// for a given type.
        /// </summary>
        /// <param name="list">The collection to add the instances to.</param>
        /// <param name="type">The type of the instance to create.</param>
        /// <param name="amount">The number of times an object should be added to the collection.</param>
        /// <param name="args">The arguments for the constructor.</param>
        public static void Add(this IList list, Type type, int amount, params object[] args)
            => Add(list, type, args.Select(arg => arg.GetType()).ToArray(), amount, args);

        /// <summary>
        /// Adds many object instance to the collection by the specified amount, invoking the same constructor
        /// for a given type.
        /// </summary>
        /// <param name="list">The collection to add the instances to.</param>
        /// <param name="type">The type of the instance to create.</param>
        /// <param name="signature">The signature of the constructor to invoke.</param>
        /// <param name="amount">The number of times an object should be added to the collection.</param>
        /// <param name="args">The arguments for the constructor.</param>
        public static void Add(this IList list, Type type, Type[] signature, int amount, params object[] args)
        {
            ConstructorInfo ctor;

            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            if ((ctor = type.GetConstructor(signature)) is null)
            {
                throw new ArgumentException(string.Format(CtorWithoutSignature, type, signature.Length));
            }

            for (int c = 0; c < amount; c++)
            {
                list.Add(ctor.Invoke(args));
            }
        }

        /// <summary>
        /// Adds the objects returned by the specified <paramref name="function"/> by the specified amount
        /// to the collection.
        /// </summary>
        /// <param name="list">The collection to add to.</param>
        /// <param name="function">The function that generates the objects to add to the collection.</param>
        /// <param name="amount">The amount of times to call the function and add the result to the collection</param>
        public static void Add(this IList list, Func<object> function, int amount)
        {
            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; c++)
            {
                list.Add(function.Invoke());
            }
        }

        /// <summary>
        /// Adds a value by the specified amount to the collection.
        /// </summary>
        /// <typeparam name="TSource">The base type of the collection</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="value">The value to add to the collection many times.</param>
        /// <param name="amount">The amount of times to call the function and add the result to the collection</param>
        public static void Add<TSource>(this ICollection<TSource> collection, TSource value, int amount)
        {
            if (collection.IsReadOnly)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; c++)
            {
                collection.Add(value);
            }
        }

        /// <summary>
        /// Adds the objects returned by the specified <paramref name="function"/> by the specified amount
        /// to the collection.
        /// </summary>
        /// <typeparam name="TSource">The base type of the collection</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="function">The function that generates the objects to add to the collection.</param>
        /// <param name="amount">The amount of times to call the function and add the result to the collection</param>
        public static void Add<TSource>(this ICollection<TSource> collection, Func<TSource> function, int amount)
        {
            if (collection.IsReadOnly)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; c++)
            {
                collection.Add(function.Invoke());
            }
        }

        public static int Count(this IEnumerable collection, Predicate<object> predicate = null)
        {
            Type genericType;
            int count;
            IEnumerator enumerator;

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (predicate is null && collection is Array)
            {
                return ((Array)collection).Length;
            }

            if (predicate is null && collection is ICollection)
            {
                return ((ICollection)collection).Count;
            }

            if (predicate is null && (genericType = collection.GetType().GetInterface(typeof(ICollection<>).Name)) != null)
            {
                return (int)genericType.GetProperty("Count").GetValue(collection);
            }

            checked
            {
                for (count = 0, enumerator = Where(collection, obj => predicate?.DynamicInvoke(obj) as bool? ?? true).GetEnumerator();
                     enumerator.MoveNext();
                     ++count) ;
            }

            return count;
        }

        public static long LongCount(this IEnumerable collection, Predicate<object> predicate = null)
        {
            long        count;
            IEnumerator enumerator;

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            checked
            {
                for (count = 0, enumerator = Where(collection, obj => predicate?.DynamicInvoke(obj) as bool? ?? true).GetEnumerator();
                     enumerator.MoveNext();
                     ++count) ;
            }

            return count;
        }

        public static BigInteger BigCount(this IEnumerable collection, Predicate<object> predicate = null)
        {
            BigInteger count;
            IEnumerator enumerator;

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            for (count = 0, enumerator = Where(collection, obj => predicate?.DynamicInvoke(obj) as bool? ?? true).GetEnumerator();
                 enumerator.MoveNext();
                 ++count) ;

            return count;
        }

        public static IEnumerable Where(IEnumerable collection, Func<object, bool> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return collection.Cast<object>().Where(predicate);
        }

        public static IEnumerable Where(IEnumerable collection, Func<object, int, bool> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return collection.Cast<object>().Where(predicate);
        }
    }
}
