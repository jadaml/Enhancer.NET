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

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class EnumerableExtensionsTest
    {
        private static ValueTuple<bool, int, int>[] _haveAtLeastSource = new ValueTuple<bool, int, int>[]
        {
            (true, 3, 2),
            (true, 3, 3),
            (false, 3, 4),
        };
        private static ValueTuple<bool, int, int>[] _haveAtMostSource = new ValueTuple<bool, int, int>[]
        {
            (false, 3, 2),
            (true, 3, 3),
            (true, 3, 4),
        };
        private static ValueTuple<bool, int, int>[] _haveExactlySource = new ValueTuple<bool, int, int>[]
        {
            (false, 3, 2),
            (true, 3, 3),
            (false, 3, 4),
        };

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveAtLeastSource))]
        public void HaveAtLeast(ValueTuple<bool, int, int> input)
        {
            if (input.Item1)
            {
                IsTrue(new object[input.Item2].HaveAtLeast(input.Item3), "Count {0} failed", input.Item3);
            }
            else
            {
                IsFalse(new object[input.Item2].HaveAtLeast(input.Item3), "Count {0} failed", input.Item3);
            }
        }

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveAtMostSource))]
        public void HaveAtMost(ValueTuple<bool, int, int> input)
        {
            if (input.Item1)
            {
                IsTrue(new object[input.Item2].HaveAtMost(input.Item3), "Count {0} failed", input.Item3);
            }
            else
            {
                IsFalse(new object[input.Item2].HaveAtMost(input.Item3), "Count {0} failed", input.Item3);
            }
        }

        [TestOf(typeof(EnumerableExtensions))]
        [TestCaseSource(nameof(_haveExactlySource))]
        public void HaveExactly(ValueTuple<bool, int, int> input)
        {
            if (input.Item1)
            {
                IsTrue(new object[input.Item2].HaveExactly(input.Item3), "Count {0} failed", input.Item3);
            }
            else
            {
                IsFalse(new object[input.Item2].HaveExactly(input.Item3), "Count {0} failed", input.Item3);
            }
        }
    }
}
