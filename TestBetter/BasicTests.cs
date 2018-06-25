using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fonlow.CommandLineGui;
using Fonlow.CommandLineGui.Robocopy;
using System.ComponentModel;
using Fonlow.CommandLine;

namespace TestBetter
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class BasicTests
    {
        public BasicTests()
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
        public void TestStringToEnum()
        {
            CopyFlags flags = CopyFlags.A | CopyFlags.D | CopyFlags.O;
            Assert.AreEqual("DAO", TypeDescriptor.GetConverter(typeof(CopyFlags)).ConvertToString(flags));

            Rashcneto ff = Rashcneto.R | Rashcneto.A | Rashcneto.S | Rashcneto.H;
            Assert.AreEqual("RASH", TypeDescriptor.GetConverter(typeof(Rashcneto)).ConvertToString(ff));
        }

        [TestMethod]
        public void TestEnumToString()
        {
            CopyFlags flags = CopyFlags.A | CopyFlags.D | CopyFlags.O;
            Assert.AreEqual((int)flags, TypeDescriptor.GetConverter(typeof(CopyFlags)).ConvertFromString("DAO"));

            Rashcneto ff = Rashcneto.R | Rashcneto.A | Rashcneto.S | Rashcneto.H;
            Assert.AreEqual((int)ff, TypeDescriptor.GetConverter(typeof(Rashcneto)).ConvertFromString("RASH"));

        }

        [TestMethod]
        public void TestAddTemplate()
        {
            CommandsTemplates.Instance.AddTemplate("FirstCommand", "DoSomething", "FirstCommand /ok", false);
            CommandsTemplates.Instance.AddTemplate("FirstCommand", "DoAnotherThing", "FirstCommand /haha", false);
            CommandsTemplates.Instance.AddTemplate("SecondCommand", "DoSomething", "SecondCommand /ok", false);
            var templatesNames = CommandsTemplates.Instance.GetTemplatesNames("FirstCommand");
            Assert.AreEqual(2, templatesNames.Count());

            CommandsTemplatesFile.Save(CommandsTemplates.Instance);

            CommandsTemplates.Instance.DeleteTemplate("FirstCommand", "DoAnotherThing");
            templatesNames = CommandsTemplates.Instance.GetTemplatesNames("FirstCommand");
            Assert.AreEqual(1, templatesNames.Count());

            var ct = new PrivateType(typeof(CommandsTemplatesFile));
            var dic = ct.InvokeStatic("Load") as CommandsTemplatesDictionary;//Singleton of CommandsTemplates is broken here, but this is fine in unit testing.
            Assert.AreEqual(2, dic.Count);

        }

        [TestMethod]
        public void TestPickyAllPropertiesAssigned()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                Option2AtMostOne = "bb",

                Option1AtLeastOne = "ab",
                Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                Option2ExactlyOne = "nnn",

                Option1All = "ttt",
                Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAllPropertiesAssignedInvalid()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                Option2AtMostOne = "bb",

           //     Option1AtLeastOne = "ab",
           //     Option2AtLeastOne = "kk",

           //     Option1ExactlyOne = "eee",
           //     Option2ExactlyOne = "nnn",

          //      Option1All = "ttt",
          //      Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(4, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAtMostOne()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                Option2AtMostOne = "bb",

                Option1AtLeastOne = "ab",
                Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                //       Option2ExactlyOne = "nnn",

                Option1All = "ttt",
                Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAtLeastOne()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                //      Option2AtMostOne = "bb",

                //      Option1AtLeastOne = "ab",
                //      Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                //       Option2ExactlyOne = "nnn",

                Option1All = "ttt",
                Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyExactlyOne()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                //      Option2AtMostOne = "bb",

                //      Option1AtLeastOne = "ab",
                //      Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                //       Option2ExactlyOne = "nnn",

                Option1All = "ttt",
                Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAll()
        {
            var options = new PickyOptions()
            {
                Option1AtMostOne = "aa",
                //      Option2AtMostOne = "bb",

                      Option1AtLeastOne = "ab",
                //      Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                //       Option2ExactlyOne = "nnn",

                Option1All = "ttt",
         //       Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestProhibitsAllPropertiesAssigned()
        {
            var options = new POptions()
            {
                Option1AtMostOne = "aa",
                Option2AtMostOne = "bb",

                Option1AtLeastOne = "ab",
                Option2AtLeastOne = "kk",

                Option1ExactlyOne = "eee",
                Option2ExactlyOne = "nnn",

                Option1All = "ttt",
                Option2All = "33fdf",
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(5, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMaxOccursOK()
        {
            var options = new MaxOccursOptions
            {
                Files = new string[] { "abc", "efg" },
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(0, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMaxOccursSilly()
        {
            var options = new MaxOccursOptionsSilly
            {
                Silly="kk"
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMaxOccurs()
        {
            var options = new MaxOccursOptions
            {
                Files = new string[] { "abc", "efg", "hkkjk", "444" },
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMinOccursExpect2But1()
        {
            var options = new MaxOccursOptions
            {
                Files = new string[] { "abc" },
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMinOccursExpect2But0()
        {
            var options = new MaxOccursOptions
            {
                Files = new string[] { },
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMinOccursExpect2ButNull()
        {
            var options = new MaxOccursOptions();

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithGoodValue()
        {
            var options = new NumericOptions
            {
                Money = 100
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(0, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithTooSmallValue()
        {
            var options = new NumericOptions
            {
                Money = 5
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithTooGreatValue()
        {
            var options = new NumericOptions
            {
                Money = 100000
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithArray()
        {
            var options = new NumericOptions
            {
                Prices = new int[] { 5, 100, 300, 5000, 100000}
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }




        [TestMethod]
        public void TestDecimalOptionsWithTooSmallValue()
        {
            var options = new DecimalOptions
            {
                Money = 5
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestDecimalOptionsWithTooGreatValue()
        {
            var options = new DecimalOptions
            {
                Money = 100000
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestDecimalOptionsWithArray()
        {
            var options = new DecimalOptions
            {
                Prices = new decimal[] { 5, 100, 300, 5000, 100000 }
            };

            var s = OptionsValidator.AnalyzeAssignedOptions(options);
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }



    }


}