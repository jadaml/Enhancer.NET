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
using static NUnit.Framework.Assert;

namespace Enhancer.Extensions.Test
{
    [TestFixture]
    public class EnumExtensionsTest
    {
        #region Unnecessarily comprehensive test enum

        [Flags]
        public enum TestEnum
        {
            Nothing,
            Flag_01 =  0b0000_0000_0000_0000_0000_0000_0000_0001,
            Flag_02 =  0b0000_0000_0000_0000_0000_0000_0000_0010,
            Flag_03 =  0b0000_0000_0000_0000_0000_0000_0000_0100,
            Flag_04 =  0b0000_0000_0000_0000_0000_0000_0000_1000,
            Flag_05 =  0b0000_0000_0000_0000_0000_0000_0001_0000,
            Flag_06 =  0b0000_0000_0000_0000_0000_0000_0010_0000,
            Flag_07 =  0b0000_0000_0000_0000_0000_0000_0100_0000,
            Flag_08 =  0b0000_0000_0000_0000_0000_0000_1000_0000,
            Flag_09 =  0b0000_0000_0000_0000_0000_0001_0000_0000,
            Flag_10 =  0b0000_0000_0000_0000_0000_0010_0000_0000,
            Flag_11 =  0b0000_0000_0000_0000_0000_0100_0000_0000,
            Flag_12 =  0b0000_0000_0000_0000_0000_1000_0000_0000,
            Flag_13 =  0b0000_0000_0000_0000_0001_0000_0000_0000,
            Flag_14 =  0b0000_0000_0000_0000_0010_0000_0000_0000,
            Flag_15 =  0b0000_0000_0000_0000_0100_0000_0000_0000,
            Flag_16 =  0b0000_0000_0000_0000_1000_0000_0000_0000,
            Flag_17 =  0b0000_0000_0000_0001_0000_0000_0000_0000,
            Flag_18 =  0b0000_0000_0000_0010_0000_0000_0000_0000,
            Flag_19 =  0b0000_0000_0000_0100_0000_0000_0000_0000,
            Flag_20 =  0b0000_0000_0000_1000_0000_0000_0000_0000,
            Flag_21 =  0b0000_0000_0001_0000_0000_0000_0000_0000,
            Flag_22 =  0b0000_0000_0010_0000_0000_0000_0000_0000,
            Flag_23 =  0b0000_0000_0100_0000_0000_0000_0000_0000,
            Flag_24 =  0b0000_0000_1000_0000_0000_0000_0000_0000,
            Flag_25 =  0b0000_0001_0000_0000_0000_0000_0000_0000,
            Flag_26 =  0b0000_0010_0000_0000_0000_0000_0000_0000,
            Flag_27 =  0b0000_0100_0000_0000_0000_0000_0000_0000,
            Flag_28 =  0b0000_1000_0000_0000_0000_0000_0000_0000,
            Flag_29 =  0b0001_0000_0000_0000_0000_0000_0000_0000,
            Flag_30 =  0b0010_0000_0000_0000_0000_0000_0000_0000,
            Flag_31 =  0b0100_0000_0000_0000_0000_0000_0000_0000,
            Flag_32 = -0b1000_0000_0000_0000_0000_0000_0000_0000,
        }

        #endregion Unnecessarily comprehensive test enum

        // Params (in order)
        // value
        // flag
        // toggle
        // (T, T) expect result
        // (T, T, bool) expect result
        private static readonly object[][] _testCases =
        {
            new object[] { TestEnum.Nothing,                                       TestEnum.Flag_08, true,  TestEnum.Flag_08,                                       TestEnum.Flag_08 },
            new object[] { TestEnum.Nothing,                                       TestEnum.Flag_08, false, TestEnum.Flag_08,                                       TestEnum.Nothing },
            new object[] { TestEnum.Flag_08,                                       TestEnum.Flag_08, true,  TestEnum.Nothing,                                       TestEnum.Flag_08 },
            new object[] { TestEnum.Flag_08,                                       TestEnum.Flag_08, false, TestEnum.Nothing,                                       TestEnum.Nothing },
            new object[] { TestEnum.Flag_02 | TestEnum.Flag_15,                    TestEnum.Flag_08, true,  TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08 },
            new object[] { TestEnum.Flag_02 | TestEnum.Flag_15,                    TestEnum.Flag_08, false, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Nothing },
            new object[] { TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08, TestEnum.Flag_08, true,  TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Nothing, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08 },
            new object[] { TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Flag_08, TestEnum.Flag_08, false, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Nothing, TestEnum.Flag_02 | TestEnum.Flag_15 | TestEnum.Nothing },
        };

        [TestCaseSource(nameof(_testCases))]
        [TestOf(typeof(EnumExtensions))]
        public void ToggleFlagTT(TestEnum value, TestEnum flag, bool state, TestEnum expectTT, TestEnum expectTTB)
        {
            AreEqual(expectTT, value.ToggleFlag(flag));
        }

        [TestCaseSource(nameof(_testCases))]
        [TestOf(typeof(EnumExtensions))]
        public void ToggleFlagTTB(TestEnum value, TestEnum flag, bool state, TestEnum expectTT, TestEnum expectTTB)
        {
            AreEqual(expectTTB, value.ToggleFlag(flag, state));
        }
    }
}
