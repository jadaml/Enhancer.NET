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

using Enhancer.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;

namespace Enhancer.Test
{
    
    
    /// <summary>
    ///This is a test class for InputDialogTest and is intended
    ///to contain all InputDialogTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InputDialogTest
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
        ///A test for InputDialog Constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void InputDialogConstructorTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for CastArray
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void CastArrayTest()
        {
            string[] array = null; // TODO: Initialize to an appropriate value
            Type[] types = null; // TODO: Initialize to an appropriate value
            object[] expected = null; // TODO: Initialize to an appropriate value
            object[] actual;
            actual = InputDialog_Accessor.CastArray(array, types);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CastArray
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void CastArrayTest1()
        {
            string[] array = null; // TODO: Initialize to an appropriate value
            Type type = null; // TODO: Initialize to an appropriate value
            object[] expected = null; // TODO: Initialize to an appropriate value
            object[] actual;
            actual = InputDialog_Accessor.CastArray(array, type);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Close
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void CloseTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.Close(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Shipout
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void ShipoutTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            target.Shipout();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Predicate<object> predicate = null; // TODO: Initialize to an appropriate value
            Type inputType = null; // TODO: Initialize to an appropriate value
            int inputCount = 0; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, predicate, inputType, inputCount);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest1()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Predicate<object> predicate = null; // TODO: Initialize to an appropriate value
            Type inputType = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, predicate, inputType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest2()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Predicate<object> predicate = null; // TODO: Initialize to an appropriate value
            Tuple<Type, string>[] fields = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, predicate, fields);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest3()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Tuple<Type, string>[] fields = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, fields);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest4()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Predicate<object> predicate = null; // TODO: Initialize to an appropriate value
            Type[] types = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, predicate, types);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest5()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Predicate<object> predicate = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, predicate);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest6()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Type inputType = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, inputType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest7()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest8()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Type inputType = null; // TODO: Initialize to an appropriate value
            int inputCount = 0; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, inputType, inputCount);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest9()
        {
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            string message = string.Empty; // TODO: Initialize to an appropriate value
            Type[] types = null; // TODO: Initialize to an appropriate value
            InputResult expected = new InputResult(); // TODO: Initialize to an appropriate value
            InputResult actual;
            actual = InputDialog.ShowDialog(owner, message, types);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ShowDialog
        ///</summary>
        [TestMethod()]
        public void ShowDialogTest10()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            IWin32Window owner = null; // TODO: Initialize to an appropriate value
            DialogResult expected = new DialogResult(); // TODO: Initialize to an appropriate value
            DialogResult actual;
            actual = target.ShowDialog(owner);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for fixHeight
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Enhancer.dll")]
        public void fixHeightTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            Control control = null; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.fixHeight(control);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Predicate
        ///</summary>
        [TestMethod()]
        public void PredicateTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            Predicate<object> expected = null; // TODO: Initialize to an appropriate value
            Predicate<object> actual;
            target.Predicate = expected;
            actual = target.Predicate;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Values
        ///</summary>
        [TestMethod()]
        public void ValuesTest()
        {
            InputDialog_Accessor target = new InputDialog_Accessor(); // TODO: Initialize to an appropriate value
            string[] expected = null; // TODO: Initialize to an appropriate value
            string[] actual;
            target.Values = expected;
            actual = target.Values;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
