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

        private static IEnumerable<TestCaseData> TypeTestCases
        {
            get
            {
                yield return new TestCaseData(typeof(SpecialList), typeof(Guid))            { ExpectedResult = null,                    TestName = "Type derives unrelated type"                      };
                yield return new TestCaseData(typeof(SpecialList), typeof(Tuple<,>))        { ExpectedResult = null,                    TestName = "Type derives unrelated generic type"              };
                yield return new TestCaseData(typeof(SpecialList), typeof(IComparable))     { ExpectedResult = null,                    TestName = "Type implements unrelated interface"              };
                yield return new TestCaseData(typeof(SpecialList), typeof(IComparable<>))   { ExpectedResult = null,                    TestName = "Type implements unrelated generic interface"      };
                yield return new TestCaseData(typeof(SpecialList), typeof(IList))           { ExpectedResult = typeof(IList),           TestName = "Type implements interface"                        };
                yield return new TestCaseData(typeof(SpecialList), typeof(IList<>))         { ExpectedResult = typeof(IList<TestType>), TestName = "Type implements generic interface definition"     };
                yield return new TestCaseData(typeof(SpecialList), typeof(IList<TestType>)) { ExpectedResult = typeof(IList<TestType>), TestName = "Type implements generic interface"                };
                yield return new TestCaseData(typeof(SpecialList), typeof(List<>))          { ExpectedResult = typeof(List<TestType>),  TestName = "Type derives from base generic type definition"   };
                yield return new TestCaseData(typeof(SpecialList), typeof(List<TestType>))  { ExpectedResult = typeof(List<TestType>),  TestName = "Type derives from base type"                      };
                yield return new TestCaseData(typeof(SpecialList), typeof(SpecialList))     { ExpectedResult = typeof(SpecialList),     TestName = "Type is it self"                                  };
                yield return new TestCaseData(new SpecialList(),   typeof(Guid))            { ExpectedResult = null,                    TestName = "Object derives from unrelated type"               };
                yield return new TestCaseData(new SpecialList(),   typeof(Tuple<,>))        { ExpectedResult = null,                    TestName = "Object derives from unrelated generic type"       };
                yield return new TestCaseData(new SpecialList(),   typeof(IComparable))     { ExpectedResult = null,                    TestName = "Object derives from unrelated interface"          };
                yield return new TestCaseData(new SpecialList(),   typeof(IComparable<>))   { ExpectedResult = null,                    TestName = "Object derives from unrelated generic interface"  };
                yield return new TestCaseData(new SpecialList(),   typeof(IList))           { ExpectedResult = typeof(IList),           TestName = "Object derives from interface"                    };
                yield return new TestCaseData(new SpecialList(),   typeof(IList<>))         { ExpectedResult = typeof(IList<TestType>), TestName = "Object derives from generic interface definition" };
                yield return new TestCaseData(new SpecialList(),   typeof(IList<TestType>)) { ExpectedResult = typeof(IList<TestType>), TestName = "Object derives from generic interface"            };
                yield return new TestCaseData(new SpecialList(),   typeof(List<>))          { ExpectedResult = typeof(List<TestType>),  TestName = "Object derives from base generic type definition" };
                yield return new TestCaseData(new SpecialList(),   typeof(List<TestType>))  { ExpectedResult = typeof(List<TestType>),  TestName = "Object derives from base type"                    };
                yield return new TestCaseData(new SpecialList(),   typeof(SpecialList))     { ExpectedResult = typeof(SpecialList),     TestName = "Object is an instance of its own type"            };
                yield return new TestCaseData(typeof(IList<>),     typeof(List<>))          { ExpectedResult = null,                    TestName = "Interface derives from descendant"                };
            }
        }

        [Test(TestOf = typeof(TypeExtensions))]
        public void GetGenericTypeError()
        {
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(null, typeof(IList<>)));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType<List>(null, typeof(IList<>)));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(new object(), null));
            Throws<ArgumentNullException>(() => TypeExtensions.GetGenericType(new List(), null));
        }

        [TestCaseSource(nameof(TypeTestCases))]
        [TestOf(typeof(TypeExtensions))]
        public Type GetGenericTypeTest(object value, Type ancestor)
        {
            if (value is Type type)
            {
                return TypeExtensions.GetGenericType(type, ancestor);
            }
            if (value is SpecialList list)
            {
                return TypeExtensions.GetGenericType((SpecialList)value, ancestor);
            }
            return TypeExtensions.GetGenericType(value.GetType(), ancestor);
        }
    }
}
