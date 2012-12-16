using Enhancer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;

namespace Enhancer.Test
{
    
    
    /// <summary>
    ///This is a test class for ExtensionMethodsTest and is intended
    ///to contain all ExtensionMethodsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionMethodsTest
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
        ///A test for Scale
        ///</summary>
        [TestMethod()]
        public void ScaleTest()
        {
            Rect rect = new Rect(.2, .2, 10.3, 10.3);
            double scalar = .5F;
            Rect expected = new Rect(.1, .1, 5.15, 5.15);
            Rect actual;
            actual = ExtensionMethods.Scale(rect, scalar);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ResizeToContent
        ///</summary>
        [TestMethod()]
        public void ResizeToContentTest()
        {
            Window window = null; // TODO: Initialize to an appropriate value
            ExtensionMethods.ResizeRule rule = new ExtensionMethods.ResizeRule(); // TODO: Initialize to an appropriate value
            ExtensionMethods.ResizeToContent(window, rule);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Refresh
        ///</summary>
        [TestMethod()]
        public void RefreshTest()
        {
            UIElement uiElement = null; // TODO: Initialize to an appropriate value
            Action method = null; // TODO: Initialize to an appropriate value
            object[] args = null; // TODO: Initialize to an appropriate value
            ExtensionMethods.Refresh(uiElement, method, args);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LastSpecifierChar
        ///</summary>
        [TestMethod()]
        public void LastSpecifierCharTest()
        {
            string str = "A \\quick 'brown' fox jumped over the lazy dog.";
            char d = '\0';
            Assert.AreEqual('z', ExtensionMethods_Accessor.LastSpecifierChar(str, 'z', d), "Couldn't find any 'z'!");
            Assert.AreEqual(d, ExtensionMethods_Accessor.LastSpecifierChar(str, '?', d), "Not the default character returned");
            Assert.AreEqual(d, ExtensionMethods_Accessor.LastSpecifierChar(str, 'q', d), "Not the default character returned");
            Assert.AreEqual(d, ExtensionMethods_Accessor.LastSpecifierChar(str, 'b', d), "Not the default character returned");
        }

        /// <summary>
        ///A test for IsTrue
        ///</summary>
        [TestMethod()]
        public void IsTrueTest()
        {
            Nullable<bool> l = new Nullable<bool>();
            Assert.IsTrue(!l.HasValue && !ExtensionMethods.IsTrue(l), "l isn't empty or not returns false.");
            l = false;
            Assert.IsFalse(ExtensionMethods.IsTrue(l), "l isn't false.");
            l = true;
            Assert.IsTrue(ExtensionMethods.IsTrue(l), "l isn't true.");
        }

        /// <summary>
        ///A test for IsFalse
        ///</summary>
        [TestMethod()]
        public void IsFalseTest()
        {
            Nullable<bool> l = new Nullable<bool>();
            Assert.IsTrue(!l.HasValue && !ExtensionMethods.IsFalse(l), "l isn't empty or not returns false.");
            l = false;
            Assert.IsTrue(ExtensionMethods.IsFalse(l), "l isn't false.");
            l = true;
            Assert.IsFalse(ExtensionMethods.IsFalse(l), "l isn't true.");
        }

        /// <summary>
        ///A test for FormatToString
        ///</summary>
        [TestMethod()]
        public void FormatToStringTest()
        {
            TimeSpan timeSpan = new TimeSpan(1, 14, 0, 31, 19);
            Assert.AreEqual(TimeSpan.Zero.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "d':'hh':'mm':'ss'.'fff"));
            Assert.AreEqual(TimeSpan.Zero.ToString("hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "[d':']hh':'mm':'ss'.'fff"));
            Assert.AreEqual(TimeSpan.Zero.ToString("mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "[d':'][hh':']mm':'ss'.'fff"));
            Assert.AreEqual(TimeSpan.Zero.ToString("d'.'ss'.'fff"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "d'.'[hh':'mm':']ss'.'fff"));
            Assert.AreEqual(TimeSpan.Zero.ToString("'.'fff"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "[d':'][hh':'][mm':'][ss]'.'fff"));
            Assert.AreEqual(TimeSpan.Zero.ToString("mm':'ss"),
                ExtensionMethods.FormatToString(TimeSpan.Zero, "[d':'][hh':']mm':'ss['.'fff]"));

            Assert.AreEqual(timeSpan.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "d':'hh':'mm':'ss'.'fff"));
            Assert.AreEqual(timeSpan.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "[d':']hh':'mm':'ss'.'fff"));
            Assert.AreEqual(timeSpan.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "[d':'][hh':']mm':'ss'.'fff"));
            Assert.AreEqual(timeSpan.ToString("d'.'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "d'.'[hh':'mm':']ss'.'fff"));
            Assert.AreEqual(timeSpan.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "[d':'][hh':'][mm':'][ss]'.'fff"));
            Assert.AreEqual(timeSpan.ToString("d':'hh':'mm':'ss'.'fff"),
                ExtensionMethods.FormatToString(timeSpan, "[d':'][hh':']mm':'ss['.'fff]"));
        }

        /// <summary>
        ///A test for FirstSpecifierChar
        ///</summary>
        [TestMethod()]
        public void FirstSpecifierCharTest()
        {
            string str = "A \\quick 'brown' fox jumped over the lazy dog.";
            char d = '\0';
            Assert.AreEqual('z', ExtensionMethods_Accessor.FirstSpecifierChar(str, 'z', d));
            Assert.AreEqual(d, ExtensionMethods_Accessor.FirstSpecifierChar(str, '?', d));
            Assert.AreEqual(d, ExtensionMethods_Accessor.FirstSpecifierChar(str, 'q', d));
            Assert.AreEqual(d, ExtensionMethods_Accessor.FirstSpecifierChar(str, 'b', d));
        }

        private void AddHelper(ICollection list)
        {
            Assert.AreEqual(10, list.Count);
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    //Assert.AreNotSame(list[i], list[j]);
                }
            }
        }

        /// <summary>
        ///A test for AddMany
        ///</summary>
        [TestMethod()]
        public void AddManyTest()
        {
            ArrayList list = new ArrayList();

            ExtensionMethods.AddMany(list, typeof(object), 10);
            Assert.AreEqual(10, list.Count);
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    Assert.AreNotSame(list[i], list[j]);
                }
            }
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            ArrayList list = new ArrayList();
            ExtensionMethods.Add(list, delegate() { return new object(); }, 10);
            Assert.AreEqual(10, list.Count);
            for (int i = 0; i < list.Count - 1; ++i)
            {
                for (int j = i + 1; j < list.Count; ++j)
                {
                    Assert.AreNotSame(list[i], list[j]);
                }
            }
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddGenericTest()
        {
            List<int> list = new List<int>();
            int i = 0;
            ExtensionMethods.Add<int>(list, delegate() { return i++; }, 10);
            Assert.AreEqual(10, list.Count);
            for (i = 0; i < list.Count; ++i)
                Assert.AreEqual<int>(i, list[i]);

            list.Clear();
            ExtensionMethods.Add<int>(list, 5, 10);
            Assert.AreEqual(10, list.Count);
            for (i = 0; i < list.Count; ++i)
                Assert.AreEqual<int>(5, list[i]);
        }
    }
}
