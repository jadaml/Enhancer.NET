﻿/* Copyright (c) 2018, 2020, Ádám L. Juhász
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

using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NUnit.Framework.Assert;
using static System.Activator;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class FancyFormatterProviderTest
    {
        public class StringConstFunc
        {
            private Func<string> _func;

            public StringConstFunc(Func<string> func)
            {
                _func = func;
            }

            public string Invoke()
            {
                return _func();
            }

            public override string ToString()
            {
                return $"\"{_func()}\"";
            }
        }

        public enum TestEnum : short
        {
            TooMuchDoughnutCanGiveYouStomachPain = 0x8ED,
            DotNETFramework4,
            GTK3,
        }

        private enum UInt08 : byte   { }
        private enum SInt08 : sbyte  { }
        private enum UInt16 : ushort { }
        private enum SInt16 : short  { }
        private enum UInt32 : uint   { }
        private enum SInt32 : int    { }
        private enum UInt64 : ulong  { }
        private enum SInt64 : long   { }

        private const string _genericCat = "Format Provider Boilerplate";
        private const string _boolCat    = "Boolean Format";
        private const string _enumCat    = "Enumeration Format";
        private const string _numbCat    = "Number Format";

        private static object[][] _boolParams =
        {
            new object[] { new StringConstFunc(() => FancyFormatProvider.On()),                "OF",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.Off()),               "OF",                 false },
            new object[] { new StringConstFunc(() => FancyFormatProvider.On().ToLower()),      "of",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.Off().ToLower()),     "of",                 false },
            new object[] { new StringConstFunc(() => FancyFormatProvider.Yes()),               "YN",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.No()),                "YN",                 false },
            new object[] { new StringConstFunc(() => FancyFormatProvider.Yes().ToLower()),     "yn",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.No().ToLower()),      "yn",                 false },
            new object[] { new StringConstFunc(() => FancyFormatProvider.True()),              "TF",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.False()),             "TF",                 false },
            new object[] { new StringConstFunc(() => FancyFormatProvider.True().ToLower()),    "tf",                 true  },
            new object[] { new StringConstFunc(() => FancyFormatProvider.False().ToLower()),   "tf",                 false },
            new object[] { new StringConstFunc(() => "1"),                                     "10",                 true  },
            new object[] { new StringConstFunc(() => "0"),                                     "10",                 false },
            new object[] { new StringConstFunc(() => "↑"),                                     "UD",                 true  },
            new object[] { new StringConstFunc(() => "↓"),                                     "UD",                 false },
            new object[] { new StringConstFunc(() => "↑"),                                     "ud",                 true  },
            new object[] { new StringConstFunc(() => "↓"),                                     "ud",                 false },
            new object[] { new StringConstFunc(() => bool.TrueString),                         "G",                  true  },
            new object[] { new StringConstFunc(() => bool.FalseString),                        "G",                  false },
            new object[] { new StringConstFunc(() => bool.TrueString.ToLower()),               "g",                  true  },
            new object[] { new StringConstFunc(() => bool.FalseString.ToLower()),              "g",                  false },
            new object[] { new StringConstFunc(() => bool.TrueString),                         null,                 true  },
            new object[] { new StringConstFunc(() => bool.FalseString),                        null,                 false },
            new object[] { new StringConstFunc(() => "foo[]"),                                 "[foo[[]]][bar[[]]]", true  },
            new object[] { new StringConstFunc(() => "bar[]"),                                 "[foo[[]]][bar[[]]]", false },
            new object[] { new StringConstFunc(() => "foo[]"),                                 "[foo[[]]]",          true  },
            new object[] { new StringConstFunc(() => ""),                                      "[foo[[]]]",          false },
        };

        private static object[][] _defenumParams =
        {
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, null },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "G" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "g" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "F" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "f" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "D" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "d" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "X" },
            new object[] { TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "x" },
        };

        private static object[][] _enumParams =
        {
            new object[] { "Too Much Doughnut Can Give You Stomach Pain", TestEnum.TooMuchDoughnutCanGiveYouStomachPain, "N" },

            new object[] { "Dot NET Framework 4", TestEnum.DotNETFramework4, "N" },
            new object[] { "GTK 3",               TestEnum.GTK3,             "N" },

            new object[] { "2287", TestEnum.GTK3, "D1" },
            new object[] { "2287", TestEnum.GTK3, "d1" },
            new object[] { "8EF",  TestEnum.GTK3, "X1" },
            new object[] { "8ef",  TestEnum.GTK3, "x1" },

            new object[] { "00002287", TestEnum.GTK3, "D8" },
            new object[] { "000008EF", TestEnum.GTK3, "X8" },
        };

        private static object[][] _enumLenPars =
        {
            new object[] { 2,  typeof(UInt08), "X0" },
            new object[] { 2,  typeof(SInt08), "X0" },
            new object[] { 3,  typeof(UInt08), "D0" },
            new object[] { 3,  typeof(SInt08), "D0" },
            new object[] { 4,  typeof(UInt16), "X0" },
            new object[] { 4,  typeof(SInt16), "X0" },
            new object[] { 5,  typeof(UInt16), "D0" },
            new object[] { 5,  typeof(SInt16), "D0" },
            new object[] { 8,  typeof(UInt32), "X0" },
            new object[] { 8,  typeof(SInt32), "X0" },
            new object[] { 10, typeof(UInt32), "D0" },
            new object[] { 10, typeof(SInt32), "D0" },
            new object[] { 16, typeof(UInt64), "X0" },
            new object[] { 16, typeof(SInt64), "X0" },
            new object[] { 20, typeof(UInt64), "D0" },
            new object[] { 20, typeof(SInt64), "D0" },
        };

        private static object[][] _defNumbPars =
        {
            new object[] { sbyte     .MinValue,        "N"  },
            new object[] { short     .MinValue,        "N"  },
            new object[] { int       .MinValue,        "N"  },
            new object[] { long      .MinValue,        "N"  },
            new object[] { byte      .MaxValue,        "N"  },
            new object[] { ushort    .MaxValue,        "N"  },
            new object[] { uint      .MaxValue,        "N"  },
            new object[] { ulong     .MaxValue,        "N"  },
            new object[] { long      .MaxValue,        null },
            new object[] { ulong     .MaxValue,        null },
            new object[] { TaskStatus.RanToCompletion, "G"  },
            new object[] { TaskStatus.RanToCompletion, "g"  },
            new object[] { TaskStatus.RanToCompletion, "X"  },
            new object[] { TaskStatus.RanToCompletion, "x"  },
            new object[] { TaskStatus.RanToCompletion, "D"  },
            new object[] { TaskStatus.RanToCompletion, "d"  },
            new object[] { TaskStatus.RanToCompletion, "F"  },
            new object[] { TaskStatus.RanToCompletion, "f"  },
        };

        private static object[][] _numbBytePars =
        {
            new object[] { "0.00\x00A0B",        -1,             "B"      },
            new object[] { "1.00\x00A0B",        1,              "B"      },
            new object[] { "1.00\x00A0B",        1,              "b"      },
            new object[] { "1.00\x00A0KiB",      1024,           "B"      },
            new object[] { "1.02\x00A0kB",       1024,           "b"      },
            new object[] { "1,024.00\x00A0B",    1024,           "b20000" },
            new object[] { "1,024.00\x00A0B",    1024,           "b20000" },
            new object[] { "16.00\x00A0EiB",     ulong.MaxValue, "B"      },
            new object[] { "18.45\x00A0EB",      ulong.MaxValue, "b"      },
            new object[] { "16,384.00\x00A0PiB", ulong.MaxValue, "B20000" },
            new object[] { "18,446.74\x00A0PB",  ulong.MaxValue, "b20000" },
        };

        [Test(TestOf = typeof(FancyFormatProvider))]
        [Category(_genericCat)]
        public void GetProvider()
        {
            AreSame(FancyFormatProvider.Provider, FancyFormatProvider.Provider.GetFormat(typeof(ICustomFormatter)));
            IsNull(FancyFormatProvider.Provider.GetFormat(null));
        }

        [Test(TestOf = typeof(FancyFormatProvider))]
        [Category(_genericCat)]
        public void NullFormat()
        {
            AreEqual(string.Empty, FancyFormatProvider.Provider.Format("{0}", null));
        }

        [TestCaseSource(nameof(_boolParams), Category = _boolCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void BoolFormat(StringConstFunc reference, string format, bool value)
        {
            AreEqual(reference.Invoke(), FancyFormatProvider.Provider.Format(format, value));
        }

        [Test(TestOf = typeof(FancyFormatProvider))]
        [Category(_boolCat)]
        public void InvalidBoolFormat()
        {
            Throws<FormatException>(() => FancyFormatProvider.Provider.Format("INVALID", true));
        }

        [TestCaseSource(nameof(_defenumParams), Category = _enumCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void DefaultEnumFormats(TestEnum value, string format)
        {
            AreEqual(value.ToString(format), FancyFormatProvider.Provider.Format(format, value));
        }

        [TestCaseSource(nameof(_enumParams), Category = _enumCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void EnumFormat(string expect, TestEnum value, string format)
        {
            AreEqual(expect, FancyFormatProvider.Provider.Format(format, value, CultureInfo.GetCultureInfo("en-US")));
        }

        [TestCaseSource(nameof(_enumLenPars), Category = _enumCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void EnumLenFormat(int digits, Type enumType, string format)
        {
            if (!enumType.IsEnum)
            {
                Inconclusive("The specified type isn't an enumeration type: {0}", enumType);
            }

            AreEqual(new string('0', digits), FancyFormatProvider.Provider.Format(format, CreateInstance(enumType)));
        }

        [Test(TestOf = typeof(FancyFormatProvider))]
        [Category(_enumCat)]
        public void InvalidEnumFormat()
        {
            Throws<FormatException>(() => FancyFormatProvider.Provider.Format("INVALID", TestEnum.GTK3));
            Throws<FormatException>(() => FancyFormatProvider.Provider.Format("DEX", TestEnum.GTK3));
            // HACK: This is not a requirement of the (nonexistent) specification.
            Throws<FormatException>(() => FancyFormatProvider.Provider.Format("?", TestEnum.GTK3));
        }

        [TestCaseSource(nameof(_defNumbPars), Category = _numbCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void DefaultNumberFormat(IFormattable value, string format)
        {
            AreEqual(value.ToString(format, null), FancyFormatProvider.Provider.Format(format, value));
        }

        [TestCaseSource(nameof(_numbBytePars), Category = _numbCat)]
        [TestOf(typeof(FancyFormatProvider))]
        public void NumberFormat(string expect, object value, string format)
        {
            AreEqual(expect, FancyFormatProvider.Provider.Format(format, value, CultureInfo.GetCultureInfo("en-US")));
        }

        [Test(TestOf = typeof(FancyFormatProvider))]
        [Category(_genericCat)]
        public void DefaultFormat()
        {
            IFormattable formattable = Substitute.For<IFormattable>();
            object obj = Substitute.For<object>();
            FancyFormatProvider.Provider.Format(null, formattable);
            FancyFormatProvider.Provider.Format(null, obj);
            formattable.DidNotReceive().ToString();
            formattable.ReceivedWithAnyArgs(1).ToString(default, default);
            obj.Received(1).ToString();
        }
    }
}
