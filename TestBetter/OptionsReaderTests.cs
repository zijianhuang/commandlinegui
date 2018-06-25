using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fonlow.CommandLine;

namespace TestBetter
{
    [TestClass]
    public class OptionsReaderTests
    {
        [TestMethod]
        public void TestParseOptions()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/F DoSomething /Intp 1234 /filters f1 \"Akk kB\" f3 \"kke fff\" /byteP 123", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.IsTrue(String.IsNullOrEmpty(s));
            Assert.AreEqual("DoSomething", options.Function);
            Assert.AreEqual(1234, options.IntP);
            //   Assert.AreEqual(123, options.ByteP);
            Assert.AreEqual(4, options.Filters.Length);
            Assert.AreEqual(123, options.ByteP);
            Assert.IsFalse(options.Help);
        }

        [TestMethod]
        public void TestBool()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Help", options);
            Assert.IsTrue(options.Help);
            Assert.IsTrue(String.IsNullOrEmpty(s));
        }

        [TestMethod]
        public void TestBoolWithExtraValues()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Help Redundant", options);
            Assert.IsFalse(String.IsNullOrEmpty(s));

            Assert.IsTrue(options.Help);
        }

        [TestMethod]
        public void TestArgs()
        {
            string[] args = new string[] { "/F", "DoSomething" };
            var options = new Options();
            var s = OptionsReader.ReadOptions(args, options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.IsTrue(String.IsNullOrEmpty(s));
            Assert.AreEqual("DoSomething", options.Function);

        }

        [TestMethod]
        public void TestArgsWithSpace()
        {
            string[] args = new string[] { "/F", "Do Something" };
            var options = new Options();
            var s = OptionsReader.ReadOptions(args, options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.IsTrue(String.IsNullOrEmpty(s));
            Assert.AreEqual("Do Something", options.Function);

        }


        [TestMethod]
        public void TestInvalidInt()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Intp 12U34 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(0, options.IntP);
        }

        [TestMethod]
        public void TestEnum()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/oe hello ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(MyEnum.Hello, options.OkEnum);

            MyEnum em = MyEnum.Hello | MyEnum.World;
            System.Diagnostics.Debug.WriteLine(em.ToString());

            var em1 = (MyEnum)Enum.Parse(typeof(MyEnum), "Hello, World");
            Assert.AreEqual(em, em1);

            var em2 = (MyEnum)Enum.Parse(typeof(MyEnum), "Hello,World");
            Assert.AreEqual(em, em2);
        }

        [TestMethod]
        public void TestEnumFlags()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/OkEnum hello,World ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(MyEnum.Hello | MyEnum.World, options.OkEnum);
        }

        [TestMethod]
        public void TestEnumFlagsInQuotedString()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/OkEnum \"hello, World\" ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(MyEnum.Hello | MyEnum.World, options.OkEnum);
        }

        [TestMethod]
        public void TestInvalidEnum()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/oe kkkk ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(MyEnum.None, options.OkEnum);
        }

        [TestMethod]
        public void TestInvalidByte()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/bytep 1234 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(0, options.ByteP);
        }

        [TestMethod]
        public void TestInvalidSByte()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/sbytep -200 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(0, options.SbyteP);
        }

        [TestMethod]
        public void TestNumericTypes()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalp 38.1234 /duration -1.79769313E+308 /floatp:3.402e38 /intp=-2147483648 /uintp=4294967295 /ushortp 65535 /longp -9223372036854775808 /ulongp 18446744073709551615", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(38.1234m, options.DecimalP);
            Assert.AreEqual(-1.79769313E+308, options.DoubleP);
            Assert.AreEqual(3.402e38f, options.FloatP);
            Assert.AreEqual(-2147483648, options.IntP);
            Assert.AreEqual(4294967295, options.UintP);
            Assert.AreEqual(65535, options.UShortP);
            Assert.AreEqual(long.MinValue, options.LongP);
            Assert.AreEqual(ulong.MaxValue, options.UlongP);
        }

        [TestMethod]
        public void TestNumericTypesArray()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalsp 38.1234  1234 /floatsp:3.402e38 1234  /intp=-2147483648 /floatsp:2234 /uintsp=4294967295 3233", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(2, options.DecimalsP.Length);
            Assert.AreEqual(3, options.FloatsP.Length);
            Assert.AreEqual(2, options.UintsP.Length);
        }

        [TestMethod]
        public void TestNumericTypesArrayWithInvalidValues()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalsp 38.1234  1234 /floatsp:3.402e38 1234  /intp=-2147483648 /floatsp:2234 /uintsp=-4294967295 3233", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual(2, options.DecimalsP.Length);
            Assert.AreEqual(3, options.FloatsP.Length);
            Assert.IsNull(options.UintsP);
        }

        [TestMethod]
        public void TestChar()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp a ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual('a', options.CharP);
        }

        [TestMethod]
        public void TestCharInChinese()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp 中 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual('中', options.CharP);
        }

        [TestMethod]
        public void TestInvalidChar()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp aa ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.AreEqual('\0', options.CharP);
        }



    }


}
