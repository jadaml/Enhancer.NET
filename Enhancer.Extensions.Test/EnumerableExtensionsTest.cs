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
using static NUnit.Framework.Assert;
using static System.Linq.Enumerable;
using static System.Math;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
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
    }
}
