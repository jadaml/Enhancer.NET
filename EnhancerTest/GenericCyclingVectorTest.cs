using Enhancer.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Enhancer.Test
{
    
    
    /// <summary>
    ///This is a test class for GenericCyclingVectorTest and is intended
    ///to contain all GenericCyclingVectorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GenericCyclingVectorTest
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
        ///A test for Add
        ///</summary>
        public void AddTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T item = default(T); // TODO: Initialize to an appropriate value
            target.Add(item);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void AddTest()
        {
            AddTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Clear
        ///</summary>
        public void ClearTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            target.Clear();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void ClearTest()
        {
            ClearTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Contains
        ///</summary>
        public void ContainsTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T item = default(T); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Contains(item);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ContainsTest()
        {
            ContainsTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for CopyTo
        ///</summary>
        public void CopyToTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T[] array = null; // TODO: Initialize to an appropriate value
            int arrayIndex = 0; // TODO: Initialize to an appropriate value
            target.CopyTo(array, arrayIndex);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void CopyToTest()
        {
            CopyToTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetEnumerator
        ///</summary>
        public void GetEnumeratorTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            IEnumerator<T> expected = null; // TODO: Initialize to an appropriate value
            IEnumerator<T> actual;
            actual = target.GetEnumerator();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void GetEnumeratorTest()
        {
            GetEnumeratorTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        public void RemoveTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T item = default(T); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Remove(item);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void RemoveTest()
        {
            RemoveTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Resize
        ///</summary>
        public void ResizeTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            ulong maxSize1 = 0; // TODO: Initialize to an appropriate value
            target.Resize(maxSize1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void ResizeTest()
        {
            ResizeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Resize
        ///</summary>
        public void ResizeTest1Helper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            long maxSize1 = 0; // TODO: Initialize to an appropriate value
            target.Resize(maxSize1);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        [TestMethod()]
        public void ResizeTest1()
        {
            ResizeTest1Helper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for peek
        ///</summary>
        public void peekTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = target.peek();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void peekTest()
        {
            peekTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for pop
        ///</summary>
        public void popTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            actual = target.pop();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void popTest()
        {
            popTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for push
        ///</summary>
        public void pushTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            T value = default(T); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.push(value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void pushTest()
        {
            pushTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Count
        ///</summary>
        public void CountTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.Count;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void CountTest()
        {
            CountTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for IsReadOnly
        ///</summary>
        public void IsReadOnlyTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.IsReadOnly;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void IsReadOnlyTest()
        {
            IsReadOnlyTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        public void ItemTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            ulong i = 0; // TODO: Initialize to an appropriate value
            T expected = default(T); // TODO: Initialize to an appropriate value
            T actual;
            target[i] = expected;
            actual = target[i];
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void ItemTest()
        {
            ItemTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for MaxSize
        ///</summary>
        public void MaxSizeTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            ulong expected = 0; // TODO: Initialize to an appropriate value
            ulong actual;
            target.MaxSize = expected;
            actual = target.MaxSize;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void MaxSizeTest()
        {
            MaxSizeTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Size
        ///</summary>
        public void SizeTestHelper<T>()
        {
            long maxSize = 0; // TODO: Initialize to an appropriate value
            GenericCyclingVector<T> target = new GenericCyclingVector<T>(maxSize); // TODO: Initialize to an appropriate value
            ulong actual;
            actual = target.Size;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void SizeTest()
        {
            SizeTestHelper<GenericParameterHelper>();
        }
    }
}
