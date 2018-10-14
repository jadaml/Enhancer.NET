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
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="random"/> or <paramref name="list"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="offset"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="offset"/> plus <paramref name="count"/> overflows
        /// the number of elements in the <paramref name="list"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="random"/> or <paramref name="list"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="offset"/> is less than zero or greater than
        /// the number of elements in the <paramref name="list"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="random"/> or <paramref name="list"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="offset"/> is less than zero or greater than the
        /// number of elements in the <paramref name="list"/>.
        /// </exception>
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
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="random"/> or <paramref name="list"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// If <paramref name="offset"/> is less than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="offset"/> plus <paramref name="count"/> overflows
        /// the number of elements in the <paramref name="list"/>.
        /// </exception>
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
        /// Adds the <paramref name="value"/> by the specified
        /// <paramref name="amount"/> of times to the collection.
        /// </summary>
        /// <param name="list">The collection to add to.</param>
        /// <param name="value">The value to add to the collection.</param>
        /// <param name="amount">
        /// The amount of times to add the <paramref name="value"/> to the
        /// collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="list"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="list"/> is read-only or is fixed in size.
        /// </exception>
        public static void Add(this IList list, object value, int amount)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(FixedSizeList);
            }

            if (list.IsReadOnly)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; ++c)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Adds many object instance of the specified <paramref name="type"/>
        /// to the collection by the specified <paramref name="amount"/>,
        /// invoking the constructor matching the types of the
        /// <paramref name="args"/> list.
        /// </summary>
        /// <param name="list">The collection to add the instances to.</param>
        /// <param name="type">The type of the instance to create.</param>
        /// <param name="amount">
        /// The number of times an object should be added to the collection.
        /// </param>
        /// <param name="args">The arguments for the constructor.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="list"/> or <paramref name="type"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="type"/> does not have a constructor
        /// matching the argument types of the <paramref name="args"/> list.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="list"/> is read-only or is fixed in size.
        /// </exception>
        public static void Add(this IList list, Type type, int amount, params object[] args)
            => Add(list, type, args.Select(arg => arg?.GetType() ?? typeof(object)).ToArray(), amount, args);

        /// <summary>
        /// Adds many object instance of the specified <paramref name="type"/>
        /// to the collection by the specified <paramref name="amount"/>,
        /// invoking the constructor matching the
        /// <paramref name="signature"/> type list and passing
        /// <paramref name="args"/> list as the arguments.
        /// </summary>
        /// <param name="list">The collection to add the instances to.</param>
        /// <param name="type">The type of the instance to create.</param>
        /// <param name="signature">The signature of the constructor to invoke.</param>
        /// <param name="amount">The number of times an object should be added to the collection.</param>
        /// <param name="args">The arguments for the constructor.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="list"/>, <paramref name="type"/> or
        /// <paramref name="signature"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If the specified <paramref name="type"/> does not have a constructor
        /// with the specified <paramref name="signature"/> list.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="list"/> is read-only or is fixed in size.
        /// </exception>
        public static void Add(this IList list, Type type, Type[] signature, int amount, params object[] args)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (signature == null)
            {
                throw new ArgumentNullException(nameof(signature));
            }

            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(FixedSizeList);
            }

            if (list.IsReadOnly)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            ConstructorInfo ctor;

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
        /// Adds the objects returned by the specified
        /// <paramref name="function"/> by the specified
        /// <paramref name="amount"/> to the collection.
        /// </summary>
        /// <param name="list">The collection to add to.</param>
        /// <param name="function">
        /// The function that generates the objects to add to the collection.
        /// </param>
        /// <param name="amount">
        /// The amount of times to call the function and add the result to the collection
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="list"/> or <paramref name="function"/> is
        /// <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// If <paramref name="list"/> is read-only or is fixed in size.
        /// </exception>
        public static void Add(this IList list, Func<object> function, int amount)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            if (list.IsFixedSize)
            {
                throw new InvalidOperationException(FixedSizeList);
            }

            if (list.IsReadOnly)
            {
                throw new InvalidOperationException(ReadOnlyCollection);
            }

            for (int c = 0; c < amount; c++)
            {
                list.Add(function.Invoke());
            }
        }

        /// <summary>
        /// Counts the number of elements where <paramref name="predicate"/> is
        /// <see langword="true"/> in an <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection who's elements should be counted.
        /// </param>
        /// <param name="predicate">
        /// <para>
        /// The condition for when to count an element.
        /// If the predicate returns <see langword="true"/> the element will be
        /// counted, and skipped if it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// A <see langword="null"/> value is equivalent to a predicate that
        /// always returns <see langword="true"/>.
        /// </para>
        /// </param>
        /// <returns>
        /// The number of elements in the <paramref name="collection"/> to which
        /// <paramref name="predicate"/> returns <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
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
                return (int)genericType.GetProperty(nameof(ICollection<object>.Count)).GetValue(collection);
            }

            checked
            {
                for (count = 0, enumerator = GetEnumeratorByPredicate(collection, predicate);
                     enumerator.MoveNext();
                     ++count) ;
            }

            return count;
        }

        /// <summary>
        /// Counts the number of elements where <paramref name="predicate"/> is
        /// <see langword="true"/> in an <see cref="IEnumerable"/> and returns
        /// it as <see cref="long"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection who's elements should be counted.
        /// </param>
        /// <param name="predicate">
        /// <para>
        /// The condition for when to count an element.
        /// If the predicate returns <see langword="true"/> the element will be
        /// counted, and skipped if it returns <see langword="false"/>.
        /// </para>
        /// A <see langword="null"/> value is equivalent to a predicate that
        /// always returns <see langword="true"/>.
        /// </param>
        /// <returns>
        /// The number of elements in the <paramref name="collection"/> to which
        /// <paramref name="predicate"/> returns <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
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
                for (count = 0, enumerator = GetEnumeratorByPredicate(collection, predicate);
                     enumerator.MoveNext();
                     ++count) ;
            }

            return count;
        }

        /// <summary>
        /// Counts the number of elements where <paramref name="predicate"/> is
        /// <see langword="true"/> in an <see cref="IEnumerable"/> and returns
        /// it as a <see cref="BigInteger"/>.
        /// </summary>
        /// <param name="collection">
        /// The collection who's elements should be counted.
        /// </param>
        /// <param name="predicate">
        /// <para>
        /// The condition for when to count an element.
        /// If the predicate returns <see langword="true"/> the element will be
        /// counted, and skipped if it returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// A <see langword="null"/> value is equivalent to a predicate that
        /// always returns <see langword="true"/>.
        /// </para>
        /// </param>
        /// <returns>
        /// The number of elements in the <paramref name="collection"/> to which
        /// <paramref name="predicate"/> returns <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the <paramref name="collection"/> is <see langword="null"/>.
        /// </exception>
        public static BigInteger BigCount(this IEnumerable collection, Predicate<object> predicate = null)
        {
            BigInteger count;
            IEnumerator enumerator;

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            for (count = 0, enumerator = GetEnumeratorByPredicate(collection, predicate);
                 enumerator.MoveNext();
                 ++count) ;

            return count;
        }

        private static IEnumerator GetEnumeratorByPredicate(IEnumerable enumerable, Predicate<object> predicate)
        {
            if (predicate is null)
            {
                return enumerable.GetEnumerator();
            }

            return Where(enumerable, obj => predicate(obj)).GetEnumerator();
        }

        /// <summary>
        /// Filters a sequence of values based on a <paramref name="predicate"/>.
        /// </summary>
        /// <param name="collection">
        /// An <see cref="IEnumerable"/> to filter.
        /// </param>
        /// <param name="predicate">
        /// A function to test each elements for a condition.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable"/> that contains elements from the input
        /// sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="collection"/> or
        /// <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable Where(this IEnumerable collection, Func<object, bool> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return collection.Cast<object>().Where<object>(predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate. Each element's
        /// index is used in the logic of the predicate function.
        /// </summary>
        /// <param name="collection">
        /// An <see cref="IEnumerable"/> to filter.
        /// </param>
        /// <param name="predicate">
        /// A function to test each source element for a condition. The second
        /// parameter of the function represents the index of the source
        /// element.
        /// </param>
        /// <returns>
        /// An <see cref="IEnumerable"/> that contains elements from the input
        /// sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If either <paramref name="collection"/> or
        /// <paramref name="predicate"/> is <see langword="null"/>.
        /// </exception>
        public static IEnumerable Where(this IEnumerable collection, Func<object, int, bool> predicate)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return collection.Cast<object>().Where<object>(predicate);
        }
    }
}
