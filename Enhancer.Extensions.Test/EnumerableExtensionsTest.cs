/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Extensions.Test.
 *
 * Enhancer.Extensions.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Extensions.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Extensions.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using static NUnit.Framework.Assert;
using static System.Linq.Enumerable;
using static System.Math;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        private class DummyList : IList
        {
            public object this[int index]
            {
                get => throw new NotImplementedException();
                set => throw new NotImplementedException();
            }

            public bool IsReadOnly => true;

            public bool IsFixedSize => false;

            public int Count => 0;

            public object SyncRoot => null;

            public bool IsSynchronized => false;

            public int Add(object value) => 0;

            public void Clear() { }

            public bool Contains(object value) => false;

            public void CopyTo(Array array, int index) { }

            public IEnumerator GetEnumerator() => new object[0].GetEnumerator();

            public int IndexOf(object value) => -1;

            public void Insert(int index, object value) => throw new NotSupportedException();

            public void Remove(object value) => throw new NotSupportedException();

            public void RemoveAt(int index) => throw new NotSupportedException();
        }

        [ExcludeFromCodeCoverage]
        private class TestCollection<T> : ICollection<T>
        {
            public int Count { get; }

            bool ICollection<T>.IsReadOnly => throw new NotImplementedException();

            public TestCollection(int count)
            {
                Count = count;
            }

            void ICollection<T>.Add(T item)
            {
                throw new NotImplementedException();
            }

            void ICollection<T>.Clear()
            {
                throw new NotImplementedException();
            }

            bool ICollection<T>.Contains(T item)
            {
                throw new NotImplementedException();
            }

            void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }

            bool ICollection<T>.Remove(T item)
            {
                throw new NotImplementedException();
            }
        }

        private static object[][] _haveAtLeastSource = new object[][]
        {
            new object[] { true, 3, 2 },
            new object[] { true, 3, 3 },
            new object[] { false, 3, 4 },
        };
        private static object[][] _haveAtMostSource = new object[][]
        {
            new object[] { false, 3, 2 },
            new object[] { true, 3, 3 },
            new object[] { true, 3, 4 },
        };
        private static object[][] _haveExactlySource = new object[][]
        {
            new object[] { false, 3, 2 },
            new object[] { true, 3, 3 },
            new object[] { false, 3, 4 },
        };

        private static readonly Random rnd = Substitute.For<Random>();
        private static readonly Dictionary<string, int> _dictTest = new Dictionary<string, int>()
        {
            ["Apple"]    = 8,
            ["Coconut"]  = 16,
            ["Enhancer"] = 2,
            ["Banana"]   = 9001,
            ["Madness"]  = 9001,
        };

        [TestCase(0, 3, 1)]
        [TestCase(0, 3, 4)]
        [TestOf(typeof(EnumerableExtensions))]
        public void IsAnyOfTest(int start, int count, int needle)
        {
            IEnumerable<int> collection = Range(start, count);

            AreEqual(collection.Contains(needle), EnumerableExtensions.IsAnyOf(needle, collection));
            AreEqual(collection.Contains(needle), EnumerableExtensions.IsAnyOf(needle, collection.ToArray()));
        }

        [TestCase(0, 3, 1)]
        [TestCase(0, 3, 4)]
        [TestOf(typeof(EnumerableExtensions))]
        public void IsNeitherTest(int start, int count, int needle)
        {
            IEnumerable<int> collection = Range(start, count);

            AreEqual(!collection.Contains(needle), EnumerableExtensions.IsNeitherOf(needle, collection));
            AreEqual(!collection.Contains(needle), EnumerableExtensions.IsNeitherOf(needle, collection.ToArray()));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveAtLeastError()
        {
            Throws<ArgumentNullException>(() => EnumerableExtensions.HaveAtLeast(null, 0));
        }

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveAtLeastSource))]
        public void HaveAtLeast(bool expect, int count, int value)
        {
            if (expect)
            {
                IsTrue(new object[count].HaveAtLeast(value), "Count {0} failed", value);
                IsTrue(new List<object>(new object[count]).HaveAtLeast(value), "Count {0} failed", value);
                IsTrue(new TestCollection<object>(count).HaveAtLeast(value), "Count {0} failed", value);
                IsTrue(Range(0, count).HaveAtLeast(value), "Count {0} failed", value);
            }
            else
            {
                IsFalse(new object[count].HaveAtLeast(value), "Count {0} failed", value);
                IsFalse(new List<object>(new object[count]).HaveAtLeast(value), "Count {0} failed", value);
                IsFalse(new TestCollection<object>(count).HaveAtLeast(value), "Count {0} failed", value);
                IsFalse(Range(0, count).HaveAtLeast(value), "Count {0} failed", value);
            }
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveAtMostError()
        {
            Throws<ArgumentNullException>(() => EnumerableExtensions.HaveAtMost(null, 0));
        }

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveAtMostSource))]
        public void HaveAtMost(bool expect, int count, int value)
        {
            if (expect)
            {
                IsTrue(new object[count].HaveAtMost(value), "Count {0} failed", value);
                IsTrue(new List<object>(new object[count]).HaveAtMost(value), "Count {0} failed", value);
                IsTrue(new TestCollection<object>(count).HaveAtMost(value), "Count {0} failed", value);
                IsTrue(Range(0, count).HaveAtMost(value), "Count {0} failed", value);
            }
            else
            {
                IsFalse(new object[count].HaveAtMost(value), "Count {0} failed", value);
                IsFalse(new List<object>(new object[count]).HaveAtMost(value), "Count {0} failed", value);
                IsFalse(new TestCollection<object>(count).HaveAtMost(value), "Count {0} failed", value);
                IsFalse(Range(0, count).HaveAtMost(value), "Count {0} failed", value);
            }
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveExactlyError()
        {
            Throws<ArgumentNullException>(() => EnumerableExtensions.HaveExactly(null, 0));
        }

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveExactlySource))]
        public void HaveExactly(bool expect, int count, int value)
        {
            if (expect)
            {
                IsTrue(new object[count].HaveExactly(value), "Count {0} failed", value);
                IsTrue(new List<object>(new object[count]).HaveExactly(value), "Count {0} failed", value);
                IsTrue(new TestCollection<object>(count).HaveExactly(value), "Count {0} failed", value);
                IsTrue(Range(0, count).HaveExactly(value), "Count {0} failed", value);
            }
            else
            {
                IsFalse(new object[count].HaveExactly(value), "Count {0} failed", value);
                IsFalse(new List<object>(new object[count]).HaveExactly(value), "Count {0} failed", value);
                IsFalse(new TestCollection<object>(count).HaveExactly(value), "Count {0} failed", value);
                IsFalse(Range(0, count).HaveExactly(value), "Count {0} failed", value);
            }
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void RandomElementError()
        {
            Throws<ArgumentNullException>(() => EnumerableExtensions.RandomElement<int>(null, rnd));
            Throws<ArgumentNullException>(() => EnumerableExtensions.RandomElement(new int[0], null));
            Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.RandomElement(new int[0], rnd, -1));
            Throws<ArgumentException>(() => EnumerableExtensions.RandomElement(new int[2], rnd, 1, 2));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void RandomElement()
        {
            rnd.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(x => (int)x[0] + Abs((int)x[0] - (int)x[1]) / 2);

            AreEqual(2, EnumerableExtensions.RandomElement(Range(0, 4).ToArray(), rnd, 1));
            AreEqual(2, EnumerableExtensions.RandomElement(Range(0, 5).ToArray(), rnd, 1, 3));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void SelectError()
        {
            Throws<ArgumentNullException>(() => EnumerableExtensions.Select(null, new int[0]));
            Throws<ArgumentNullException>(() => EnumerableExtensions.Select<int>(rnd, null));
            Throws<ArgumentOutOfRangeException>(() => EnumerableExtensions.Select(rnd, new int[0], -1));
            Throws<ArgumentException>(() => EnumerableExtensions.Select(rnd, new int[2], 1, 2));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Select()
        {
            rnd.Next(Arg.Any<int>(), Arg.Any<int>()).Returns(x => (int)x[0] + Abs((int)x[0] - (int)x[1]) / 2);

            AreEqual(2, EnumerableExtensions.Select(rnd, Range(0, 4).ToArray(), 1));
            AreEqual(2, EnumerableExtensions.Select(rnd, Range(0, 5).ToArray(), 1, 3));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALOErrorNull()
        {
            IList list = null;
            Throws<ArgumentNullException>(() => list.Add(42, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALOErrorFixedSize()
        {
            Throws<InvalidOperationException>(() => new int[0].Add(42, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALOErrorReadOnly()
        {
            Throws<InvalidOperationException>(() => new DummyList().Add(42, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALO()
        {
            IList list = new List<int>() { 2, 3, 4, };
            list.Add(42, 8);
            CollectionAssert.AreEqual(new int[] { 2, 3, 4, 42, 42, 42, 42, 42, 42, 42, 42 }, list);
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALTErrorNull()
        {
            Throws<ArgumentNullException>(() => (null as List<object>).Add(typeof(DateTime), 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALTErrorNull2()
        {
            Throws<ArgumentNullException>(() => new List<object>().Add(null as Type, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALTErrorFixedSize()
        {
            Throws<InvalidOperationException>(() => new object[0].Add(typeof(int), 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALTErrorReadOnly()
        {
            Throws<InvalidOperationException>(() => new DummyList().Add(typeof(int), 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALTErrorCtor()
        {
            Throws<ArgumentException>(() => new List<object>().Add(typeof(int), 8, null, null, null));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALT()
        {
            List<object> list = new List<object>() { 42 };

            list.Add(typeof(DateTime), 8, 1900, 12, 30, 12, 34, 56, 789);

            CollectionAssert.AreEqual(new object[]
            {
                42,
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
            }, list);
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorNull()
        {
            Throws<ArgumentNullException>(() => (null as List<object>).Add(typeof(DateTime), new Type[0], 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorNull2()
        {
            Throws<ArgumentNullException>(() => new List<object>().Add(null as Type, new Type[0], 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorNull3()
        {
            Throws<ArgumentNullException>(() => new List<object>().Add(typeof(DateTime), null as Type[], 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorFixedSize()
        {
            Throws<InvalidOperationException>(() => new object[0].Add(typeof(int), new Type[0], 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorReadOnly()
        {
            Throws<InvalidOperationException>(() => new DummyList().Add(typeof(int), new Type[0], 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALTErrorCtor()
        {
            Throws<ArgumentException>(() => new List<object>().Add(typeof(int), new Type[] { typeof(int), typeof(int), typeof(int) }, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add4ALT()
        {
            List<object> list = new List<object>() { 42 };

            list.Add(typeof(DateTime), 8, 1900, 12, 30, 12, 34, 56, 789);

            CollectionAssert.AreEqual(new object[]
            {
                42,
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
            }, list);
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALFErrorNull()
        {
            Throws<ArgumentNullException>(() => (null as IList).Add(() => null, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALFErrorNull2()
        {
            Throws<ArgumentNullException>(() => (new List<int>()).Add(null as Func<object>, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALFFixedSize()
        {
            Throws<InvalidOperationException>(() => new object[0].Add(() => null, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALFReadOnly()
        {
            Throws<InvalidOperationException>(() => new DummyList().Add(() => null, 8));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Add3ALF()
        {
            List<object> list = new List<object>() { 42 };

            ((IList)list).Add(() => new DateTime(1900, 12, 30, 12, 34, 56, 789), 8);

            CollectionAssert.AreEqual(new object[]
            {
                42,
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
                new DateTime(1900, 12, 30, 12, 34, 56, 789),
            }, list);
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountErrorNull()
        {
            Throws<ArgumentNullException>(() => (null as IEnumerable).Count());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountArray()
        {
            AreEqual(8, (new int[8] as IEnumerable).Count());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountCollection()
        {
            ICollection collection = Substitute.For<ICollection>();

            collection.Count.Returns(8);

            AreEqual(8, (collection as IEnumerable).Count());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountTypedCollection()
        {
            ICollection<int> collection = Substitute.For<ICollection<int>>();

            collection.Count.Returns(8);

            AreEqual(8, (collection as IEnumerable).Count());
        }

        private struct DummyEnumerator : IEnumerator
        {
            public object Current => null;

            public bool MoveNext() => true;

            public void Reset() { }
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
#if DEBUG
        [Ignore("Takes ridiculously long time to execute. Needs to be reworked.")]
#endif
        [Category("DoNotCover")]
        [Timeout(15_000)]
        public void CountEnumerableOverflow()
        {
            IEnumerable enumerable = Substitute.For<IEnumerable>();

            enumerable.GetEnumerator().Returns(new DummyEnumerator());

            Throws<OverflowException>(() => enumerable.Count());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountEnumerable()
        {
            IEnumerable enumerable = Substitute.For<IEnumerable>();
            IEnumerator enumerator = Substitute.For<IEnumerator>();

            enumerable.GetEnumerator().Returns(enumerator);
            enumerator.MoveNext().Returns(true, true, true, true, true, true, true, true, false);
            enumerator.Current.Returns(
                ci => 1,
                ci => 2,
                ci => 3,
                ci => 4,
                ci => 5,
                ci => 6,
                ci => 7,
                ci => 8,
                ci => throw new InvalidOperationException());

            AreEqual(8, enumerable.Count());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void CountEnumerablePredicate()
        {
            IEnumerable enumerable = Substitute.For<IEnumerable>();
            IEnumerator enumerator = Substitute.For<IEnumerator>();

            enumerable.GetEnumerator().Returns(enumerator);
            enumerator.MoveNext().Returns(true, true, true, true, true, true, true, true, false);
            enumerator.Current.Returns(
                ci => 1,
                ci => 2,
                ci => 3,
                ci => 4,
                ci => 5,
                ci => 6,
                ci => 7,
                ci => 8,
                ci => throw new InvalidOperationException());

            AreEqual(4, enumerable.Count(obj => obj as int? < 5));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void LongCountNullError()
        {
            Throws<ArgumentNullException>(() => (null as IEnumerable).LongCount());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void LongCount()
        {
            AreEqual(8, Range(1, 8).LongCount());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void LongCountPredicate()
        {
            AreEqual(4, Range(1, 8).LongCount(obj => obj as int? < 5));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void BigCountNullError()
        {
            Throws<ArgumentNullException>(() => (null as IEnumerable).BigCount());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void BigCount()
        {
            AreEqual((BigInteger)8, Range(1, 8).BigCount());
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void BigCountPredicate()
        {
            AreEqual((BigInteger)4, Range(1, 8).BigCount(obj => obj as int? < 5));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void WhereNullError1()
        {
            Throws<ArgumentNullException>(() => (null as IEnumerable).Where(obj => true));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void WhereNullError2()
        {
            Throws<ArgumentNullException>(() => (new int[0] as IEnumerable).Where(null as Func<object, bool>));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void Where()
        {
            IEnumerable enumerable = Substitute.For<IEnumerable>();
            IEnumerator enumerator = Substitute.For<IEnumerator>();

            enumerable.GetEnumerator().Returns(enumerator);
            enumerator.MoveNext().Returns(true, true, true, true, true, true, true, true, false);
            enumerator.Current.Returns(
                ci => 1,
                ci => 2,
                ci => 3,
                ci => 4,
                ci => 5,
                ci => 6,
                ci => 7,
                ci => 8,
                ci => throw new InvalidOperationException());

            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, enumerable.Where(obj => obj as int? < 6 && obj as int? > 2));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void WhereIndexNullError1()
        {
            Throws<ArgumentNullException>(() => (null as IEnumerable).Where((obj, i) => true));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void WhereIndexNullError2()
        {
            Throws<ArgumentNullException>(() => (new int[0] as IEnumerable).Where(null as Func<object, int, bool>));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void WhereIndex()
        {
            IEnumerable enumerable = Substitute.For<IEnumerable>();
            IEnumerator enumerator = Substitute.For<IEnumerator>();

            enumerable.GetEnumerator().Returns(enumerator);
            enumerator.MoveNext().Returns(true, true, true, true, true, true, true, true, false);
            enumerator.Current.Returns(
                ci => 1,
                ci => 2,
                ci => 3,
                ci => 4,
                ci => 5,
                ci => 6,
                ci => 7,
                ci => 8,
                ci => throw new InvalidOperationException());

            CollectionAssert.AreEqual(new[] { 3, 4, 5 }, enumerable.Where((obj, i) => i < 5 && obj as int? > 2));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipArrayNull1()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip((Func<object[], object>)null));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipArrayNull2()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip(ar => null as object, null as object[]));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipArrayNull3()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip(ar => null as object, new[] { null as IEnumerable }));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipEnumNull1()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip((Func<object[], object>)null, Repeat<IEnumerable>(null, 0)));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipEnumNull2()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip(ar => null as object, null as IEnumerable<IEnumerable>));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipEnumNull3()
        {
            Throws(typeof(ArgumentNullException), () => EnumerableExtensions.Zip(ar => null as object, Repeat<IEnumerable>(null, 1)));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipArrayNothing()
        {
            CollectionAssert.AreEqual(new bool[0], EnumerableExtensions.Zip(o => true));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipArray()
        {
            CollectionAssert.AreEqual(
                Repeat(18, 5),
                EnumerableExtensions.Zip(objs => objs.Cast<int>().Sum(),
                new[] {  1, 2, 3, 4,  5 },
                new[] {  5, 4, 3, 2,  1, 5 },
                new[] {  2, 4, 6, 8, 10, 5, 4 },
                new[] { 10, 8, 6, 4,  2 }));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipEnumNothing()
        {
            CollectionAssert.AreEqual(new bool[0], EnumerableExtensions.Zip(o => true, new object[0].Cast<IEnumerable>()));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void ZipEnum()
        {
            CollectionAssert.AreEqual(
                Repeat(18, 5),
                EnumerableExtensions.Zip(objs => objs.Cast<int>().Sum(),
                new[]
                {
                    new[] {  1, 2, 3, 4,  5 },
                    new[] {  5, 4, 3, 2,  1, 5 },
                    new[] {  2, 4, 6, 8, 10, 5, 4 },
                    new[] { 10, 8, 6, 4,  2 },
                }.AsEnumerable()));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void FindKeyForValueArEx()
        {
            Throws<ArgumentException>(() => EnumerableExtensions.FindKeyForValue(_dictTest, 9000));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void FindKeyForValueInvalid()
        {
            Throws<InvalidOperationException>(() => EnumerableExtensions.FindKeyForValue(_dictTest, 9001));
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void FindKeyForValue()
        {
            AreEqual("Enhancer", EnumerableExtensions.FindKeyForValue(_dictTest, 2));
        }
    }
}
