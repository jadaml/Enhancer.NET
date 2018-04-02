/* Copyright (c) 2018, Ádám L. Juhász
 *
 * This file is part of EnhancerTest.
 *
 * EnhancerTest is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * EnhancerTest is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with EnhancerTest.  If not, see <http://www.gnu.org/licenses/>.
 */

using Enhancer.WPF;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Enhancer.Test
{
    
    
    /// <summary>
    ///This is a test class for BooleanNotConverterTest and is intended
    ///to contain all BooleanNotConverterTest Unit Tests
    ///</summary>
    [TestClass()]
    public class BooleanNotConverterTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ConvertBack
        ///</summary>
        [TestMethod()]
        public void ConvertBackTest()
        {
            BooleanNotConverter target = new BooleanNotConverter(); // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            Type targetType = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            CultureInfo culture = null; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.ConvertBack(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Convert
        ///</summary>
        [TestMethod()]
        public void ConvertTest()
        {
            BooleanNotConverter target = new BooleanNotConverter(); // TODO: Initialize to an appropriate value
            object value = null; // TODO: Initialize to an appropriate value
            Type targetType = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            CultureInfo culture = null; // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Convert(value, targetType, parameter, culture);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
