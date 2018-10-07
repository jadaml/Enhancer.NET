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
    public class IndirectAttachedValueTest
    {
        public int Value;

        private AttachedValue<int> _attachedValue;

        public IndirectAttachedValueTest()
        {
            _attachedValue = new AttachedValue<int>(() => this.Value);
        }

        [SetUp]
        public void Setuo()
        {
            Value = 28;
            _attachedValue.Rollback();
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void ReferredObject()
        {
            AreSame(this, _attachedValue.ReferenceObject);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void GetReference()
        {
            AreEqual(Value = 42, _attachedValue.Reference);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void CommitChange()
        {
            _attachedValue.Value = 42;
            _attachedValue.Commit();

            AreEqual(42, _attachedValue.Reference);
            AreEqual(42, Value);
        }

        [Test(TestOf = typeof(AttachedValue<>))]
        public void CommitUnchange()
        {
            _attachedValue.Value = 28;
            _attachedValue.Commit();

            AreEqual(28, _attachedValue.Reference);
            AreEqual(28, Value);
        }
    }
}
