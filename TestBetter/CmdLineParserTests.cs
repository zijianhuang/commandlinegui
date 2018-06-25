using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fonlow.CommandLine;

namespace TestBetter
{
    [TestClass]
    public class CmdLineParserTests
    {
        [TestMethod]
        public void TestCommandAndArgs()
        {
            Options options = new Options();
            var parser = new CommandLineParser(options);
            parser.Parse("abcde.exe  /F DoSomething /Intp 1234 /filters f1 \"Akk kB\" f3 \"kke fff\" /byteP 123 /filters abc /filters kkk");
            Assert.IsFalse(parser.HasErrors);
            Assert.AreEqual("DoSomething", options.Function);
            Assert.AreEqual(1234, options.IntP);
            //   Assert.AreEqual(123, options.ByteP);
            Assert.AreEqual(6, options.Filters.Length);
            Assert.AreEqual(123, options.ByteP);
            Assert.IsFalse(options.Help);
            Assert.AreEqual("abcde.exe", parser.ExecutablePath);
        }

        [TestMethod]
        public void TestCommandLineNoExecutable()
        {
            Options options = new Options();
            var parser = new CommandLineParser(options);
            parser.Parse("/F DoSomething /Intp 1234 /filters f1 \"Akk kB\" f3 \"kke fff\" /byteP 123 /h  /fi abc /fil kkk", false);
            Assert.IsFalse(parser.HasErrors);
            Assert.AreEqual("DoSomething", options.Function);
            Assert.AreEqual(1234, options.IntP);
            //   Assert.AreEqual(123, options.ByteP);
            Assert.AreEqual(6, options.Filters.Length);
            Assert.AreEqual(123, options.ByteP);
            Assert.IsTrue(options.Help);
            Assert.IsNull(parser.ExecutablePath);
        }

        [TestMethod]
        public void TestArgs()
        {
            string[] args = new string[] { "/filters", "f1", "Akk kB", "f3", "kke fff" };//.net console app will rip of double quotes, so all args won't be quoted string.
            Options options = new Options();
            var parser = new CommandLineParser(options);
            parser.Parse(args);
            Assert.IsFalse(parser.HasErrors);
            Assert.AreEqual("f1", options.Filters[0]);
            Assert.AreEqual(4, options.Filters.Length);
            Assert.AreEqual("Akk kB", options.Filters[1]);
            Assert.IsNull(parser.ExecutablePath);
        }




        [TestMethod]
        public void TestPickyAllPropertiesAssigned()
        {
            var options = new PickyOptions();
            
            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa /Option2AtMostOne:bb /Option1AtLeastOne:ab /Option2AtLeastOne:kk /Option1ExactlyOne:eee /Option2ExactlyOne:nnn /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAllPropertiesAssignedInvalid()
        {
            var options = new PickyOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa /Option2AtMostOne:bb  ", false);
            var s = parser.ErrorMessage;

            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(4, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAtMostOne()
        {
            var options = new PickyOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa /Option2AtMostOne:bb /Option1AtLeastOne:ab /Option2AtLeastOne:kk /Option1ExactlyOne:eee /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAtLeastOne()
        {
            var options = new PickyOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa     /Option1ExactlyOne:eee   /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyExactlyOne()
        {
            var options = new PickyOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa  /Option1ExactlyOne:eee  /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestPickyAll()
        {
            var options = new PickyOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa  /Option1AtLeastOne:ab  /Option1ExactlyOne:eee  /Option1All:ttt ", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestProhibitsAllPropertiesAssigned()
        {
            var options = new POptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa /Option2AtMostOne:bb /Option1AtLeastOne:ab /Option2AtLeastOne:kk /Option1ExactlyOne:eee /Option2ExactlyOne:nnn /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(5, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestProhibitsAllPropertiesAssignedWithAlias()
        {
            var options = new POptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option1AtMostOne:aa /Option2AtMostOne:bb /Option1AtLeastOne:ab /Option2AtLeastOne:kk /Option1ExactlyOne:eee /O2E1:nnn /Option1All:ttt /Option2All:ggg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(5, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestProhibitsPropertiesAssigned()
        {
            var options = new POptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Option2AtLeastOne:ab /Op2eOne:hahah", false);
            var s = parser.ErrorMessage;

            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMaxOccursOK()
        {
            var options = new MaxOccursOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Files:abc efg", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.IsNull(s);
        }

        [TestMethod]
        public void TestMaxOccursSilly()
        {
            var options = new MaxOccursOptionsSilly
            {
                Silly = "kk"
            };

            var parser = new CommandLineParser(options);
            parser.Parse("/Silly:kk", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMaxOccurs()
        {
            var options = new MaxOccursOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Files:abc efg hijkdfd 444", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestMinOccursExpect2But1()
        {
            var options = new MaxOccursOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Files:abc ", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }



        [TestMethod]
        public void TestNumericOptionsWithGoodValue()
        {
            var options = new NumericOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Money:100", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.IsNull(s);
        }

        [TestMethod]
        public void TestNumericOptionsWithTooSmallValue()
        {
            var options = new NumericOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Money:5", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithTooGreatValue()
        {
            var options = new NumericOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Money:100000", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestNumericOptionsWithArray()
        {
            var options = new NumericOptions
            {
                Prices = new int[] { 5, 100, 300, 5000, 100000 }
            };

            var parser = new CommandLineParser(options);
            parser.Parse("/Prices:5 100 300 5000 100000", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }




        [TestMethod]
        public void TestDecimalOptionsWithTooSmallValue()
        {
            var options = new DecimalOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Money:5", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }

        [TestMethod]
        public void TestDecimalOptionsWithTooGreatValue()
        {
            var options = new DecimalOptions();

            var parser = new CommandLineParser(options);
            parser.Parse("/Money:100000", false);
            var s = parser.ErrorMessage;
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

            var parser = new CommandLineParser(options);
            parser.Parse("/Prices:5 100 300 5000 100000", false);
            var s = parser.ErrorMessage;
            System.Diagnostics.Debug.WriteLine(s);
            Assert.AreEqual(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
        }


    }
}
