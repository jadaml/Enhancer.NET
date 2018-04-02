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
        [TestMethod()]
        public void peekTest()
        {
            GenericCyclingVector_Accessor<GenericParameterHelper> vector =
                new GenericCyclingVector_Accessor<GenericParameterHelper>(10);
            GenericParameterHelper value = new GenericParameterHelper(1);
            vector.push(value);
            ulong preFirst = vector._first, preLength = vector._length;
            Assert.AreEqual(value, vector.peek(), "Peek gives different value.");
            Assert.AreEqual(preFirst, vector._first, "Object changed from peek.");
            Assert.AreEqual(preLength, vector._length, "Object changed from peek.");
            Assert.AreEqual(value, vector._vector[preFirst], "Object changed from peek.");
        }

        /// <summary>
        ///A test for pop
        ///</summary>
        [TestMethod()]
        public void popTest()
        {
            GenericCyclingVector_Accessor<GenericParameterHelper> vector =
                new GenericCyclingVector_Accessor<GenericParameterHelper>(10);
            ulong preFirst = vector._first;
            Assert.AreEqual(default(GenericParameterHelper), vector.pop(), "Poped some value.");
            Assert.AreEqual(preFirst, vector._first, "Possible inconsistent state of vector.");
            GenericParameterHelper value = new GenericParameterHelper(1);
            vector.push(value);
            Assert.AreEqual(value, vector.pop(), "Poped different value.");
            for (int i = 0; i < 9; ++i)
            {
                vector.push(value); vector.pop();
            }
            try
            {
                vector.push(value);
            }
            catch (Exception)
            {
                Assert.Fail("Push thrown an exception, but it shouldn't.");
            }
        }

        /// <summary>
        ///A test for push
        ///</summary>
        [TestMethod()]
        public void pushTest()
        {
            GenericCyclingVector_Accessor<GenericParameterHelper> vector =
                new GenericCyclingVector_Accessor<GenericParameterHelper>(10);
            for (int i = 0; i < 10; ++i)
            {
                GenericParameterHelper value = new GenericParameterHelper(i);
                Assert.IsTrue(vector.push(value), "Unable to push element");
                Assert.AreSame(value, vector._vector[vector._first + vector._length - 1], "The vector stored a different value.");
            }
            Assert.IsFalse(vector.push(new GenericParameterHelper(-1)), "Was able to push element");
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
        [TestMethod()]
        public void IsReadOnlyTest()
        {
            GenericCyclingVector<GenericParameterHelper> target = new GenericCyclingVector<GenericParameterHelper>(10);
            Assert.IsFalse(target.IsReadOnly, "That is not the nature of this class.");
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
