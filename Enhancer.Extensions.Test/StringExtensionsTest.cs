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
    public class StringExtensionsTest
    {
        [Test(TestOf = typeof(StringExtensions))]
        public void ToUserFriendlyString()
        {
            AreEqual("String Transformation For NET Framework 45 Works", "stringTransformation_forNETFramework45Works".ToUserFriendlyString());
        }
    }
}
