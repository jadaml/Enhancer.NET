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
using System.Linq;
using Enhancer.Extensions;
using System.Diagnostics.CodeAnalysis;
using static NUnit.Framework.Assert;

namespace Enhancer.Collections.Test
{
    [TestFixture]
    public class CyclingVectorTest
    {
        private const string _cDontCover = "DoNotCover";

        private static readonly int _initialSize = new CyclingVector<int>().Capacity;

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ConstructorOutOfRangeException()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<int>(-1));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ConstructorCapacity()
        {
            AreEqual(_initialSize + 1, new CyclingVector<int>(_initialSize + 1).Capacity);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void GetThisOutOfRange()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 4, 5, 6, };

            Throws<ArgumentOutOfRangeException>(() => _ = vector[-1]);
            Throws<ArgumentOutOfRangeException>(() => _ = vector[5]);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void GetThis()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 4, 5, 6, };

            AreEqual(4, vector[0]);
            AreEqual(5, vector[1]);
            AreEqual(6, vector[2]);

            vector.Push(vector.Pop());

            AreEqual(5, vector[0]);
            AreEqual(6, vector[1]);
            AreEqual(4, vector[2]);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SetThisOutOfRange()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 4, 5, 6, };

            Throws<ArgumentOutOfRangeException>(() => vector[-1] = 0);
            Throws<ArgumentOutOfRangeException>(() => vector[5] = 0);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SetThis()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 4, 5, 6, };

            vector.Push(vector.Pop());

            vector[2] = 0;

            CollectionAssert.AreEqual(new[] { 5, 6, 0 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void GetThisSpecialOutOfRange()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            Throws<ArgumentOutOfRangeException>(() => _ = vector[-8]);
            Throws<ArgumentOutOfRangeException>(() => _ = vector[6]);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void GetThisSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            AreEqual(2, vector[3]);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SetThisSpecialOutOfRange()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            Throws<ArgumentOutOfRangeException>(() => vector[-8] = 0);
            Throws<ArgumentOutOfRangeException>(() => vector[6]  = 0);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SetThisSpecialTypeMismatch()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            Throws<ArgumentException>(() => vector[3] = DateTime.Now);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SetThisSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ISNotReadonly()
        {
            IsFalse(new CyclingVector<int>().IsReadOnly);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void SyncRoonConsistency()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            AreSame(((ICollection)vector).SyncRoot, ((ICollection)vector).SyncRoot);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IsSynchronized()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            IsTrue(vector.IsSynchronized);
            Inconclusive("Further tests are required to test for actually being synchronized,");
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void FixedSize()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            IsFalse(vector.IsFixedSize);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CountEmpty()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            CollectionAssert.AreEqual(new int[0], vector);
            AreEqual(0, vector.Count);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Count()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8 };
            CollectionAssert.AreEqual(new[] { 5, 3, 8 }, vector);
            AreEqual(3, vector.Count);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void PeekEmpty()
        {
            CyclingVector<int> vector = new CyclingVector<int>(_initialSize);

            for (int c = _initialSize; c > 0; --c)
            {
                vector.Push(8);
                vector.Pop();
            }

            AreEqual(default(int), vector.Peek());
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Peek()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8 };

            CollectionAssert.AreEqual(new[] { 5, 3, 8 }, vector);
            AreEqual(5, vector.Peek());
            CollectionAssert.AreEqual(new[] { 5, 3, 8 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void PopEmpty()
        {
            CyclingVector<int> vector = new CyclingVector<int>(_initialSize);

            for (int c = _initialSize; c > 0; --c)
            {
                vector.Push(8);
                vector.Pop();
            }

            AreEqual(0, vector.Count);
            AreEqual(default(int), vector.Pop());
            AreEqual(0, vector.Count);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Pop()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8 };

            CollectionAssert.AreEqual(new[] { 5, 3, 8 }, vector);
            AreEqual(5, vector.Pop());
            CollectionAssert.AreEqual(new[] { 3, 8 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        [Category(_cDontCover)]
        [ExcludeFromCodeCoverage]
        public void PopReleases()
        {
            WeakReference wr = new WeakReference(new object());
            CyclingVector<object> vector = new CyclingVector<object>() { wr.Target };

            IsTrue(wr.IsAlive);

            vector.Pop();

            GC.Collect(3, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            Console.WriteLine(string.Format(FancyFormatProvider.Provider,
                @"
Is Alive: {0:YN}
Is Null:  {1:YN}".Trim(), wr.IsAlive, wr.Target is null));

            if (wr.IsAlive)
            {
                Inconclusive("Reference was not released after explicit garbage collection.");
            }
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Push()
        {
            CyclingVector<int> vector = new CyclingVector<int>();

            CollectionAssert.AreEqual(new int[0], vector);
            vector.Push(5);
            CollectionAssert.AreEqual(new[] { 5 }, vector);
            vector.Push(3);
            CollectionAssert.AreEqual(new[] { 5, 3 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void PushResize()
        {
            CyclingVector<int> vector = new CyclingVector<int>(_initialSize);

            for (int c = _initialSize; c > 0; --c) { vector.Push(5); }

            vector.Pop();

            vector.Push(6);
            vector.Push(7);

            AreEqual(_initialSize + 1, vector.Count);
            CollectionAssert.AreEqual(Enumerable.Repeat(5, _initialSize - 1).Concat(new[] { 6, 7 }), vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Add()
        {
            CyclingVector<int> vector = new CyclingVector<int>();

            CollectionAssert.AreEqual(new int[0], vector);
            vector.Add(5);
            CollectionAssert.AreEqual(new[] { 5 }, vector);
            vector.Add(3);
            CollectionAssert.AreEqual(new[] { 5, 3 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void AddResize()
        {
            CyclingVector<int> vector = new CyclingVector<int>(_initialSize);

            for (int c = _initialSize; c > 0; --c) { vector.Add(5); }

            vector.Pop();

            vector.Add(6);
            vector.Add(7);

            AreEqual(_initialSize + 1, vector.Count);
            CollectionAssert.AreEqual(Enumerable.Repeat(5, _initialSize - 1).Concat(new[] { 6, 7 }), vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ClearEmpty()
        {
            CyclingVector<object> vector = new CyclingVector<object>();

            vector.Clear();

            CollectionAssert.AreEqual(new object[0], vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Clear()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 1, 4 };

            vector.Clear();

            CollectionAssert.AreEqual(new int[0], vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        [Category(_cDontCover)]
        [ExcludeFromCodeCoverage]
        public void ClearReleases()
        {
            WeakReference[] references =
            {
                new WeakReference(new object(), true),
                new WeakReference(new object(), true),
                new WeakReference(new object(), true),
                new WeakReference(new object(), true),
            };
            CyclingVector<object> vector = new CyclingVector<object>();

            foreach (WeakReference wr in references)
            {
                vector.Add(wr.Target);
            }

            vector.Clear();

            CollectionAssert.AreEqual(new object[0], vector);

            GC.Collect(3, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();

            if (references.Any(wr => wr.IsAlive))
            {
                Inconclusive("Some references did not get released after explicit garbage collection.");
            }
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ContainsEmpty()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            IsFalse(vector.Contains(new Random().Next()));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Contains()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };
            IsTrue(vector.Contains(2));
            IsFalse(vector.Contains(9));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToNull()
        {
            Throws<ArgumentNullException>(() => new CyclingVector<object>().CopyTo(null, 0));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToNegative()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<object>().CopyTo(new object[0], -8));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToOutOfRange()
        {
            Throws<ArgumentException>(() => new CyclingVector<object>() { 5, 3, 8, 2, 4 }.CopyTo(new object[8], 5));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyTo()
        {
            int[] array;
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.Remove(4);
            vector.Push(4);

            array = new int[8];

            vector.CopyTo(array, 2);

            CollectionAssert.AreEqual(new[] { 0, 0, 5, 3, 8, 2, 4, 0 }, array);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Remove()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            IsTrue(vector.Remove(2), "Return value indicates failure; it should have succeeded.");

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveExternal()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            IsFalse(vector.Remove(7), "Return value indicates success; it should have failed.");

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveEmpty()
        {
            CyclingVector<int> vector = new CyclingVector<int>();

            IsFalse(vector.Remove(2), "Return value indicates success; it should have failed.");

            CollectionAssert.AreEqual(new int[0], vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToSpecialNullError()
        {
            Throws<ArgumentNullException>(() => (new CyclingVector<object>() as ICollection).CopyTo(null, 0));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToSpecialNegative()
        {
            Throws<ArgumentOutOfRangeException>(() => (new CyclingVector<object>() as ICollection).CopyTo(new object[0], -8));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToSpecialOutOfRange()
        {
            Throws<ArgumentException>(() => (new CyclingVector<object> { 5, 3, 6, 2, 4 } as ICollection).CopyTo(new object[8], 5));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void CopyToSpecial()
        {
            object[] array = new object[8];
            ICollection vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.CopyTo(array, 2);

            CollectionAssert.AreEqual(new object[] { null, null, 5, 3, 8, 2, 4, null }, array);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IndexOf()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 2 };

            AreEqual(3, vector.IndexOf(2));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IndexOfExternal()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };
            AreEqual(-1, vector.IndexOf(7));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<object>().Insert(-8, null));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertOutOfRange()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<int>() { 5, 3, 8, 2, 4 }.Insert(6, 42));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Insert()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 4 };

            vector.Insert(3, 2);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertResize()
        {
            CyclingVector<int> vector = new CyclingVector<int>(_initialSize);

            for (int c = _initialSize; c > 0; --c)
            {
                vector.Add(5);
            }

            vector.Insert(3, 4);

            CollectionAssert.AreEqual(Enumerable.Repeat(5, 3).Concat(new[] { 4 }).Concat(Enumerable.Repeat(5, _initialSize - 3)), vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveAtNegativeIndex()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<object>().RemoveAt(-8));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveAtOutOfRange()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<int>() { 5, 3, 8, 2, 4 }.RemoveAt(6));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveAt()
        {
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.RemoveAt(3);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void AddSpecialTypeMismatch()
        {
            Throws<ArgumentException>(() => (new CyclingVector<int>() as IList).Add(DateTime.Now));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void AddSpecial()
        {
            IList vector = new CyclingVector<int>();

            CollectionAssert.AreEqual(new int[0], vector);
            AreEqual(0, vector.Add(5));
            CollectionAssert.AreEqual(new[] { 5 }, vector);
            AreEqual(1, vector.Add(3));
            CollectionAssert.AreEqual(new[] { 5, 3 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ContainsSpecialTypeMismatch()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            IsFalse(vector.Contains(DateTime.Now));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ContainsSpecialExternal()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            IsFalse(vector.Contains(7));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void ContainsSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            IsTrue(vector.Contains(2));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IndexOfSpecialTypeMismatch()
        {
            AreEqual(-1, (new CyclingVector<int>() { 5, 3, 8, 2, 4 } as IList).IndexOf(DateTime.Now));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IndexOfSpecialExternal()
        {
            AreEqual(-1, (new CyclingVector<int>() { 5, 3, 8, 2, 4 } as IList).IndexOf(6));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void IndexOfSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 2 };

            AreEqual(3, vector.IndexOf(2));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertSpecialTypeMismatch()
        {
            Throws<ArgumentException>(() => (new CyclingVector<int>() { 5, 3, 8, 4 } as IList).Insert(3, DateTime.Now));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertSpecialNegative()
        {
            Throws<ArgumentOutOfRangeException>(() => (new CyclingVector<int>() as IList).Insert(-8, 42));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertSpecialOutOfRange()
        {
            Throws<ArgumentOutOfRangeException>(() => (new CyclingVector<int>() { 5, 3, 8, 2, 4 } as IList).Insert(6, 42));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void InsertSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 4 };

            vector.Insert(3, 2);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveSpecialTypeMismatch()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.Remove(DateTime.Now);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveSpecialExternal()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.Remove(6);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 2, 4 }, vector);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void RemoveSpecial()
        {
            IList vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            vector.Remove(2);

            CollectionAssert.AreEqual(new[] { 5, 3, 8, 4 }, vector);
        }
    }
}
