/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of Enhancer.Configuration.Test.
 *
 * Enhancer.Configuration.Test is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Enhancer.Configuration.Test is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Enhancer.Configuration.Test.  If not, see <http://www.gnu.org/licenses/>.
 */

using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class MemoryValueTest
    {
        [Test(TestOf = typeof(MemoryValue<>))]
        public void ConstructorValue()
        {
            MemoryValue<int> value = new MemoryValue<int>(28);

            AreEqual(28, value.Reference);
            IsFalse(value.IsModified);
        }

        [Test(TestOf = typeof(MemoryValue<>))]
        public void CommitChanged()
        {
            MemoryValue<int> value = new MemoryValue<int>();

            value.Value = 28;

            IsTrue(value.IsModified);

            value.Commit();

            AreEqual(28, value.Reference);
        }

        [Test(TestOf = typeof(MemoryValue<>))]
        public void Cast()
        {
            MemoryValue<int> value = 28;

            AreEqual(28, value.Value);
            AreEqual(28, value.Reference);
            IsFalse(value.IsModified);
        }
    }
}
