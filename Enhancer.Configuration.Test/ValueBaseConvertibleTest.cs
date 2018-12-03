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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class ValueBaseConvertibleTest
    {
        private class ValueTest<T> : ValueBase<T>
        {
            public override T Reference { get; protected set; }

            public ValueTest() : this(default(T)) { }
            public ValueTest(T value) => Reference = value;
        }

        [TestOf(typeof(ValueBase<>))]
        [TestCaseSource(typeof(ConversionTestCases), nameof(ConversionTestCases.Cases), new object[] { TestCases.ChangeType, typeof(ValueTest<>) })]
        public object ChangeType(ITransactionedValue value, Func<object, object> convertion, bool fails)
        {
            if (fails)
            {
                Assert.That(() => convertion(value), Throws.InstanceOf<InvalidCastException>());
                return value.Reference;
            }

            return convertion(value);
        }

        [TestOf(typeof(ValueBase<>))]
        [TestCaseSource(typeof(ConversionTestCases), nameof(ConversionTestCases.Cases), new object[] { TestCases.TypeCode, typeof(ValueTest<>) })]
        public TypeCode TypeCodeTest(IValue value) => Convert.GetTypeCode(value);
    }
}
