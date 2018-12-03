using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enhancer.Configuration.Test
{
    [TestFixture]
    public class SimpleValueConvertibleTest
    {
        [TestOf(typeof(SimpleValue<>))]
        [TestCaseSource(typeof(ConversionTestCases), nameof(ConversionTestCases.Cases), new object[] { TestCases.ChangeType, typeof(SimpleValue<>) })]
        public object ChangeType(IValue value, Func<object, object> convertion, bool fails)
        {
            if (fails)
            {
                Assert.That(() => convertion(value), Throws.InstanceOf<InvalidCastException>());
                return value.Value;
            }

            return convertion(value);
        }

        [TestOf(typeof(SimpleValue<>))]
        [TestCaseSource(typeof(ConversionTestCases), nameof(ConversionTestCases.Cases), new object[] { TestCases.TypeCode, typeof(SimpleValue<>) })]
        public TypeCode TypeCodeTest(IValue value) => Convert.GetTypeCode(value);
    }
}
