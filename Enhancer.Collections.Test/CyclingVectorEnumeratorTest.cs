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
    public class CyclingVectorEnumeratorTest
    {
        [Test]
        [TestOf("CyclingVector`1+CyclingVectorEnumerator")]
        public void EnumeratorDisposal()
        {
            IEnumerator etor = new CyclingVector<object>() { null }.GetEnumerator();

            ((IDisposable)etor).Dispose();

            Throws<ObjectDisposedException>(() => etor.MoveNext(), "MoveNext doesn't throw ObjectDisposedException when enumerator is disposed.");
            Throws<ObjectDisposedException>(() => _ = etor.Current, "Current doesn't throw ObjectDisposedException when enumerator is disposed.");
            Throws<ObjectDisposedException>(() => etor.Reset(), "Reset doesn't throw ObjectDisposedException when enumerator is disposed.");

            DoesNotThrow(() => ((IDisposable)etor).Dispose(), "Dispose throws an exception when called second time.");
        }

        [Test]
        [TestOf("CyclingVector`1+CyclingVectorEnumerator")]
        public void EnumeratorInvalid()
        {
            CyclingVector<object> vector = new CyclingVector<object>() { null };
            IEnumerator etor = vector.GetEnumerator();

            vector.Add(null);

            Assert.That(() => etor.MoveNext(), Throws.Exception);
        }

        [Test]
        [TestOf("CyclingVector`1+CyclingVectorEnumerator")]
        public void EnumeratorOverflow()
        {
            IEnumerator etor = new CyclingVector<object>() { null, null, null }.GetEnumerator();

            IsTrue(etor.MoveNext());
            IsTrue(etor.MoveNext());
            IsTrue(etor.MoveNext());
            IsFalse(etor.MoveNext());

            // Technically this should be repeated 2³² times; but that takes a lot of time.
            IsFalse(etor.MoveNext());
        }

        [Test]
        [TestOf("CyclingVector`1+CyclingVectorEnumerator")]
        public void Enumerator()
        {
            IEnumerator<int> etor;
            int i;

            int[] expect = { 5, 3, 8, 2, 4 };
            CyclingVector<int> vector = new CyclingVector<int>() { 5, 3, 8, 2, 4 };

            for (i = 0, etor = vector.GetEnumerator(); etor.MoveNext(); ++i)
            {
                AreEqual(expect[i], etor.Current, "Failed at {0}. iteration; 1st round.", i);
            }

            for (i = 0, etor.Reset(); etor.MoveNext(); ++i)
            {
                AreEqual(expect[i], etor.Current, "Failed at {0}. iteration; 2nd round.", i);
            }
        }
    }
}
