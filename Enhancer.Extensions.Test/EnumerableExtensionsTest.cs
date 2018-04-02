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
        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveAtLeast()
        {
            IsTrue (new object[3].HaveAtLeast(2), "Count 2 failed");
            IsTrue (new object[3].HaveAtLeast(3), "Count 3 failed");
            IsFalse(new object[3].HaveAtLeast(4), "Count 4 failed");
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveAtMost()
        {
            IsFalse(new object[3].HaveAtMost(2), "Count 2 failed");
            IsTrue (new object[3].HaveAtMost(3), "Count 3 failed");
            IsTrue (new object[3].HaveAtMost(4), "Count 4 failed");
        }

        [Test(TestOf = typeof(EnumerableExtensions))]
        public void HaveExactly()
        {
            IsFalse(new object[3].HaveExactly(2), "Count 2 failed");
            IsTrue (new object[3].HaveExactly(3), "Count 3 failed");
            IsFalse(new object[3].HaveExactly(4), "Count 4 failed");
        }
    }
}
