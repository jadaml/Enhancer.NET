/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Collections.Test.
 *
 * Enhancer.Collections.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Collections.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Collections.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Throws = NUnit.Framework.Throws;
using static NUnit.Framework.Assert;

namespace Enhancer.Collections.Test
{
    [TestFixture]
    public class IndexRangeTest
    {
        private const string _enumeratorName = "IndexRange+IndexEnumerator";

        private static IndexRange _dummyRange = new IndexRange(0, 0, 1);

        private static object[][] _countArgs =
        {
            new object[] { 4, 0, 3, 1 },
            new object[] { 5, 0, 8, 2 },
            new object[] { 4, 6, 15, 3 },
            new object[] { 4, 6, 17, 3 },
        };

        private static object[][] _indexValueMap =
        {
            new object[] { 3, 17, 3, 0, 3  },
            new object[] { 3, 17, 3, 1, 6  },
            new object[] { 3, 17, 3, 2, 9  },
            new object[] { 3, 17, 3, 3, 12 },
            new object[] { 3, 17, 3, 4, 15 },
        };

        private static object[][] _valueMissMap =
        {
            new object[] { 3, 17, 3, -1, 5   },
            new object[] { 3, 17, 3, -1, 128 },
            new object[] { 3, 17, 3, -1, 0   },
        };

        [TestCaseSource(nameof(_countArgs))]
        [TestOf(typeof(IndexRange))]
        public void Count(int expect, int start, int end, int increments)
        {
            AreEqual(expect, new IndexRange(start, end, increments).Count);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void CollectionReadOnly()
        {
            IsTrue(((ICollection<int>)_dummyRange).IsReadOnly);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void ListReadOnly()
        {
            IsTrue(((IList)_dummyRange).IsReadOnly);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void FixedSize()
        {
            IsTrue(((IList)_dummyRange).IsFixedSize);
        }

        [TestCaseSource(nameof(_countArgs))]
        [TestOf(typeof(IndexRange))]
        public void SpecialCount(int expect, int start, int end, int increments)
        {
            AreEqual(expect, ((ICollection)new IndexRange(start, end, increments)).Count);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SyncRootConsistency()
        {
            ICollection range = _dummyRange;

            AreSame(range.SyncRoot, range.SyncRoot);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void IndexerPropertyNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => _ = _dummyRange[-2]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void IndexerPropertyOverflow()
        {
            Throws<ArgumentOutOfRangeException>(() => _ = new IndexRange(2, 7, 1)[6]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void IndexerPropertySetNotSupported()
        {
            Throws<NotSupportedException>(() => new IndexRange(2, 7, 1)[0] = 8);
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestOf(typeof(IndexRange))]
        public void IndexerProperty(int start, int end, int increment, int index, int element)
        {
            AreEqual(element, new IndexRange(start, end, increment)[index]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialIndexerPropertyNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => _ = ((IList)_dummyRange)[-2]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialIndexerPropertyOverflow()
        {
            Throws<ArgumentOutOfRangeException>(() => _ = ((IList)new IndexRange(2, 7, 1))[6]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialIndexerPropertySetNotSupported()
        {
            Throws<NotSupportedException>(() => ((IList)new IndexRange(2, 7, 1))[0] = 8);
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestOf(typeof(IndexRange))]
        public void SpecialIndexerProperty(int start, int end, int increment, int index, int element)
        {
            AreEqual(element, ((IList)new IndexRange(start, end, increment))[index]);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void ZeroIncrementIsInvalid()
        {
            Throws<ArgumentException>(() => new IndexRange(0, 0, 0));
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestCaseSource(nameof(_valueMissMap))]
        [TestOf(typeof(IndexRange))]
        public void IndexOf(int start, int end, int increment, int index, int element)
        {
            AreEqual(index, new IndexRange(start, end, increment).IndexOf(element));
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestCaseSource(nameof(_valueMissMap))]
        [TestOf(typeof(IndexRange))]
        public void Contains(int start, int end, int increment, int index, int element)
        {
            IndexRange range = new IndexRange(start, end, increment);

            if (index == -1)
            {
                IsFalse(range.Contains(element));
            }
            else
            {
                IsTrue(range.Contains(element));
            }
        }

        [Test(TestOf = typeof(IndexRange))]
        public void CopyToNullArg()
        {
            Throws<ArgumentNullException>(() => _dummyRange.CopyTo(null, 0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void CopyToNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => _dummyRange.CopyTo(new int[0], -1));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void CopyToOverflow()
        {
            Throws<ArgumentException>(() => new IndexRange(3, 17, 3).CopyTo(new int[7], 3));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void CopyTo()
        {
            IndexRange range = new IndexRange(3, 17, 3);
            int[] array = new int[10];

            range.CopyTo(array, 3);

            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 3, 6, 9, 12, 15, 0, 0 }, array);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialIndexOfArbitrary()
        {
            AreEqual(-1, ((IList)new IndexRange(3, 17, 3)).IndexOf(DateTime.Now));
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestCaseSource(nameof(_valueMissMap))]
        [TestOf(typeof(IndexRange))]
        public void SpecialIndexOf(int start, int end, int increment, int index, int element)
        {
            AreEqual(index, ((IList)new IndexRange(start, end, increment)).IndexOf(element));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialContainsArbitrary()
        {
            IsFalse(((IList)new IndexRange(3, 17, 3)).Contains(DateTime.Now));
        }

        [TestCaseSource(nameof(_indexValueMap))]
        [TestCaseSource(nameof(_valueMissMap))]
        [TestOf(typeof(IndexRange))]
        public void SpecialContains(int start, int end, int increment, int index, int element)
        {
            IList range = new IndexRange(start, end, increment);

            if (index == -1)
            {
                IsFalse(range.Contains(element));
            }
            else
            {
                IsTrue(range.Contains(element));
            }
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialCopyToNullArg()
        {
            Throws<ArgumentNullException>(() => ((IList)_dummyRange).CopyTo(null, 0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialCopyToNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => ((IList)_dummyRange).CopyTo(new int[0], -1));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialCopyToOverflow()
        {
            Throws<ArgumentException>(() => ((IList)new IndexRange(3, 17, 3)).CopyTo(new int[7], 3));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialCopyTo()
        {
            IList range = new IndexRange(3, 17, 3);
            int[] array = new int[10];

            range.CopyTo(array, 3);

            CollectionAssert.AreEqual(new int[] { 0, 0, 0, 3, 6, 9, 12, 15, 0, 0 }, array);
        }

        [Test(TestOf = typeof(IndexRange))]
        public void Insert()
        {
            Throws<NotSupportedException>(() => ((IList<int>)_dummyRange).Insert(0, 0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void RemoveAt()
        {
            Throws<NotSupportedException>(() => ((IList<int>)_dummyRange).RemoveAt(0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void Add()
        {
            Throws<NotSupportedException>(() => ((ICollection<int>)_dummyRange).Add(0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void Clear()
        {
            Throws<NotSupportedException>(() => ((ICollection<int>)_dummyRange).Clear());
        }

        [Test(TestOf = typeof(IndexRange))]
        public void Remove()
        {
            Throws<NotSupportedException>(() => ((ICollection<int>)_dummyRange).Remove(0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialAdd()
        {
            Throws<NotSupportedException>(() => ((IList)_dummyRange).Add(0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialClear()
        {
            Throws<NotSupportedException>(() => ((IList)_dummyRange).Clear());
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialInsert()
        {
            Throws<NotSupportedException>(() => ((IList)_dummyRange).Insert(0, 0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialRemove()
        {
            Throws<NotSupportedException>(() => ((IList)_dummyRange).Remove(0));
        }

        [Test(TestOf = typeof(IndexRange))]
        public void SpecialRemoveAt()
        {
            Throws<NotSupportedException>(() => ((IList)_dummyRange).RemoveAt(0));
        }

        [Test]
        [TestOf(_enumeratorName)]
        public void EnumeratorMoveNext()
        {
            IndexRange range = new IndexRange(3, 17, 3);
            IEnumerator etor = range.GetEnumerator();
            int i;

            for (i = 0; i < 5; ++i)
            {
                IsTrue(etor.MoveNext(), "Attempt {0}", i + 1);
            }

            IsFalse(etor.MoveNext(), "Attempt {0}", ++i);
            IsFalse(etor.MoveNext(), "Attempt {0}", ++i);
        }

        [Test]
        [TestOf(_enumeratorName)]
        public void EnumeratorReset()
        {
            IndexRange range = new IndexRange(3, 17, 3);
            IEnumerator etor = range.GetEnumerator();

            etor.MoveNext();
            etor.Reset();
            etor.MoveNext();

            AreEqual(3, etor.Current);
        }

        [Test]
        [TestOf(_enumeratorName)]
        public void EnumeratorCurrentException()
        {
            IndexRange range = new IndexRange(3, 17, 3);
            IEnumerator etor = range.GetEnumerator();

            Assert.That(() => etor.Current, Throws.Exception);

            while (etor.MoveNext()) ;

            Assert.That(() => etor.Current, Throws.Exception);
        }

        [Test]
        [TestOf(_enumeratorName)]
        public void EnumeratorCurrent()
        {
            CollectionAssert.AreEqual(new int[] { 3, 6, 9, 12, 15 }, new IndexRange(3, 17, 3));
        }
    }
}
