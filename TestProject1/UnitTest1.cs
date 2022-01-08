using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApplication1;

namespace TestProject1
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCheckMoveTo()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkmoveto("moveto 10 10");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkmoveto("moveto 10");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckDrawTo()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkdrawto("drawto 10 10");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkdrawto("drawto 10");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckClear()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkclear("clear");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkclear("clear clear");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckReset()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkreset("reset");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkreset("RESET");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckRectangle()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkrectangle("rectangle 10 10");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkrectangle("rectangle 10");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckCircle()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkcircle("circle 10");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkcircle("circle 10 10");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckTriangle()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checktriangle("triangle 20 10 10 10");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checktriangle("triangle 0 10 10 20 0 10");
            Assert.AreEqual(retval2, false);

        }

        [TestMethod]
        public void TestCheckPen()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkpen("pen red");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkpen("pen purple");
            Assert.AreEqual(retval2, false);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestCheckFill()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkfill("fill on");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkfill("fill purple");
            Assert.AreEqual(retval2, false);

        }

        //[TestMethod]
        //public void TestDrawTriangle()
        //{
        //    try
        //    {
        //        CommandParser cmdParser1 = new CommandParser();
        //        cmdParser1.drawTriangle(10, 0, 20, 30);
        //        Assert.Fail("");
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Assert.Fail(ex.Message);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestCheckReservedKeywords()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkforReservedKeywords("Var");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkforReservedKeywords("invalid");
            Assert.AreEqual(retval2, false);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestCheckForProgramBlocks()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.checkforProgramBlocks("Var ");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkforProgramBlocks("moveto");
            Assert.AreEqual(retval2, false);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestEvaluateExpression()
        {
            CommandParser cmdParser1 = new CommandParser();
            int retval1 = (int)cmdParser1.evaluateExpression("1+1");
            Assert.AreEqual(retval1, 2);

            CommandParser cmdParser2 = new CommandParser();
            int retval2 = (int)cmdParser2.evaluateExpression("1+1");
            Assert.AreNotEqual(retval2, 10);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestsingleLineExecutableCommand()
        {
            CommandParser cmdParser1 = new CommandParser();
            bool retval1 = cmdParser1.singleLineExecutableCommand("moveto ");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.singleLineExecutableCommand("1+1");
            Assert.AreNotEqual(retval2, 10);

        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestcheckCommandContainsVariableName()
        {
            CommandParser cmdParser1 = new CommandParser();
            cmdParser1.variables_values.Add("myVar", "10");

            bool retval1 = cmdParser1.checkCommandContainsVariableName("myVar");
            Assert.AreEqual(retval1, true);

            CommandParser cmdParser2 = new CommandParser();
            bool retval2 = cmdParser2.checkCommandContainsVariableName("1+1");
            Assert.AreNotEqual(retval2, true);

        }
            
    }
}
