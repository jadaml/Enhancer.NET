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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace Enhancer.Collections.Test
{
    [TestFixture]
    public class CyclingVectorTest
    {
        [Test(TestOf = typeof(CyclingVector<>))]
        public void ConstructorOutOfRangeException()
        {
            Throws<ArgumentOutOfRangeException>(() => new CyclingVector<int>(-1));
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void AddValue()
        {
            CyclingVector<int> vector = new CyclingVector<int>();
            AreEqual(0, vector.Count);
            vector.Add(5);
            AreEqual(1, vector.Count);
            vector.Add(5);
            AreEqual(2, vector.Count);
            vector.Add(5);
            AreEqual(3, vector.Count);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void PopValue()
        {
            CyclingVector<int> vector = new CyclingVector<int>()
            {
                4,
                5,
                5,
            };

            AreEqual(4, vector.Pop());
            AreEqual(2, vector.Count);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void This()
        {
            CyclingVector<int> vector = new CyclingVector<int>()
            {
                4,
                5,
                6,
            };

            AreEqual(4, vector[0]);
            AreEqual(5, vector[1]);
            AreEqual(6, vector[2]);

            vector.Push(vector.Pop());

            AreEqual(5, vector[0]);
            AreEqual(6, vector[1]);
            AreEqual(4, vector[2]);
        }

        [Test(TestOf = typeof(CyclingVector<>))]
        public void Resize()
        {
            CyclingVector<int> vector = new CyclingVector<int>(2)
            {
                5,
                5,
            };

            vector.Pop();

            vector.Add(6);
            vector.Add(7);

            AreEqual(5, vector[0]);
            AreEqual(6, vector[1]);
            AreEqual(7, vector[2]);
        }
    }
}
