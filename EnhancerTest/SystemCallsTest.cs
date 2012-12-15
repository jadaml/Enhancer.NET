using Enhancer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Windows.Forms;

namespace Enhancer.Test
{


    /// <summary>
    ///This is a test class for SystemCallsTest and is intended
    ///to contain all SystemCallsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SystemCallsTest
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
        ///A test for LoadDialogResultCaption
        ///</summary>
        [TestMethod()]
        public void LoadDialogResultCaptionTest()
        {
            StringBuilder result = new StringBuilder();
            foreach (SystemCalls.DialogResultCaption caption in Enum.GetValues(typeof(SystemCalls.DialogResultCaption)))
            {
                result.AppendFormat("\t{0,-10}\t: {1}\n",
                    Enum.GetName(typeof(SystemCalls.DialogResultCaption), caption),
                    SystemCalls.LoadDialogResultCaption(caption));
            }

            Assert.AreEqual(DialogResult.Yes,
                MessageBox.Show(string.Format("Because Windows can be localized, it is imposible wether a resource that was "
                    + "loaded successfully is indeed the resource was asked for. Therefore I ask you wether these values "
                    + "seems legit to you (<requested resource>: <the resource returned>)\n\n{0}\nDoes this seems legit?",
                    result.ToString()), "Human Response Required Test", MessageBoxButtons.YesNo),
                "User claimed that the results are inconsistent.");
        }
    }
}
