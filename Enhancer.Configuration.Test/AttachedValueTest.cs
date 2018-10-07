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
using System;
using static NUnit.Framework.Assert;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class AttachedValueTest
    {
        private AttachedValue<int> _value = (AttachedValue<int>)28;

        [Test(TestOf = typeof(AttachedValue<>))]
        public void AttachedValueCast()
        {
            AreEqual(28, (int)_value);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void AttachedValueValue()
        {
            AreEqual(28, _value.Value);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void AttachedValueReference()
        {
            AreEqual(28, _value.Reference);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void AttachedValueReferencedObjectInvalid()
        {
            Throws<InvalidOperationException>(() => _ = _value.ReferenceObject);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void TransitiveAttachedValueCantBeSet()
        {
            Throws<InvalidOperationException>(() => _value.Value = 42);
        }
    }
}
