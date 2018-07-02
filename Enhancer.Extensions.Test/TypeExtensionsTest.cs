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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestType = System.Int32;
using static NUnit.Framework.CollectionAssert;
using static NUnit.Framework.Assert;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class TypeExtensionsTest
    {
        private class SpecialList : List<TestType>
        {
            public SpecialList() : base(0) { }
        }

        private static object[][] _types = new object[][]
        {
            new object[] { typeof(Guid),            null                    },
            new object[] { typeof(Tuple<,>),        null                    },
            new object[] { typeof(IComparable),     null                    },
            new object[] { typeof(IComparable<>),   null                    },
            new object[] { typeof(IList),           typeof(IList)           },
            new object[] { typeof(IList<>),         typeof(IList<TestType>) },
            new object[] { typeof(IList<TestType>), typeof(IList<TestType>) },
            new object[] { typeof(List<>),          typeof(List<TestType>)  },
            new object[] { typeof(List<TestType>),  typeof(List<TestType>)  },
            new object[] { typeof(SpecialList),     typeof(SpecialList)     },
        };

        [Test(TestOf = typeof(TypeExtensions))]
        public void GetGenericTypeError()
        {
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(null, typeof(IList<>)));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType<List>(null, typeof(IList<>)));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(new object(), null));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(new List(), null));
        }

        [TestCaseSource(nameof(_types))]
        [TestOf(typeof(TypeExtensions))]
        public void GetGenericTypeTest(Type value, Type expect)
        {
            AreEqual(expect, TypeExtensions.GetGenericType(typeof(SpecialList), value));
            AreEqual(expect, TypeExtensions.GetGenericType(new SpecialList(), value));
        }
    }
}
