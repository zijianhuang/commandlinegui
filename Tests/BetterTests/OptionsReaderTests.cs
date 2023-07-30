using System;
using Xunit;
using Fonlow.CommandLine;

namespace TestBetter
{
     
    public class OptionsReaderTests
    {
        [Fact]
        public void TestParseOptions()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/F DoSomething /Intp 1234 /filters f1 \"Akk kB\" f3 \"kke fff\" /byteP 123", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.True(String.IsNullOrEmpty(s));
            Assert.Equal("DoSomething", options.Function);
            Assert.Equal(1234, options.IntP);
            //   Assert.Equal(123, options.ByteP);
            Assert.Equal(4, options.Filters.Length);
            Assert.Equal(123, options.ByteP);
            Assert.False(options.Help);
        }

        [Fact]
        public void TestBool()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Help", options);
            Assert.True(options.Help);
            Assert.True(String.IsNullOrEmpty(s));
        }

        [Fact]
        public void TestBoolWithExtraValues()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Help Redundant", options);
            Assert.False(String.IsNullOrEmpty(s));

            Assert.True(options.Help);
        }

        [Fact]
        public void TestArgs()
        {
            string[] args = new string[] { "/F", "DoSomething" };
            var options = new Options();
            var s = OptionsReader.ReadOptions(args, options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.True(String.IsNullOrEmpty(s));
            Assert.Equal("DoSomething", options.Function);

        }

        [Fact]
        public void TestArgsWithSpace()
        {
            string[] args = new string[] { "/F", "Do Something" };
            var options = new Options();
            var s = OptionsReader.ReadOptions(args, options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.True(String.IsNullOrEmpty(s));
            Assert.Equal("Do Something", options.Function);

        }


        [Fact]
        public void TestInvalidInt()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Intp 12U34 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(0, options.IntP);
        }

        [Fact]
        public void TestEnum()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/oe hello ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(MyEnum.Hello, options.OkEnum);

            MyEnum em = MyEnum.Hello | MyEnum.World;
            System.Diagnostics.Debug.WriteLine(em.ToString());

            var em1 = (MyEnum)Enum.Parse(typeof(MyEnum), "Hello, World");
            Assert.Equal(em, em1);

            var em2 = (MyEnum)Enum.Parse(typeof(MyEnum), "Hello,World");
            Assert.Equal(em, em2);
        }

        [Fact]
        public void TestEnumFlags()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/OkEnum hello,World ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(MyEnum.Hello | MyEnum.World, options.OkEnum);
        }

        [Fact]
        public void TestEnumFlagsInQuotedString()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/OkEnum \"hello, World\" ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(MyEnum.Hello | MyEnum.World, options.OkEnum);
        }

        [Fact]
        public void TestInvalidEnum()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/oe kkkk ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(MyEnum.None, options.OkEnum);
        }

        [Fact]
        public void TestInvalidByte()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/bytep 1234 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(0, options.ByteP);
        }

        [Fact]
        public void TestInvalidSByte()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/sbytep -200 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(0, options.SbyteP);
        }

        [Fact]
        public void TestNumericTypes()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalp 38.1234 /duration -1.79769313E+308 /floatp:3.402e38 /intp=-2147483648 /uintp=4294967295 /ushortp 65535 /longp -9223372036854775808 /ulongp 18446744073709551615", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(38.1234m, options.DecimalP);
            Assert.Equal(-1.79769313E+308, options.DoubleP);
            Assert.Equal(3.402e38f, options.FloatP);
            Assert.Equal(-2147483648, options.IntP);
            Assert.Equal(4294967295, options.UintP);
            Assert.Equal(65535, options.UShortP);
            Assert.Equal(long.MinValue, options.LongP);
            Assert.Equal(ulong.MaxValue, options.UlongP);
        }

        [Fact]
        public void TestNumericTypesArray()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalsp 38.1234  1234 /floatsp:3.402e38 1234  /intp=-2147483648 /floatsp:2234 /uintsp=4294967295 3233", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(2, options.DecimalsP.Length);
            Assert.Equal(3, options.FloatsP.Length);
            Assert.Equal(2, options.UintsP.Length);
        }

        [Fact]
        public void TestNumericTypesArrayWithInvalidValues()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/Decimalsp 38.1234  1234 /floatsp:3.402e38 1234  /intp=-2147483648 /floatsp:2234 /uintsp=-4294967295 3233", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal(2, options.DecimalsP.Length);
            Assert.Equal(3, options.FloatsP.Length);
            Assert.Null(options.UintsP);
        }

        [Fact]
        public void TestChar()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp a ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal('a', options.CharP);
        }

        [Fact]
        public void TestCharInChinese()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp 中 ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal('中', options.CharP);
        }

        [Fact]
        public void TestInvalidChar()
        {
            var options = new Options();
            var s = OptionsReader.ReadOptions("/charp aa ", options);
            System.Diagnostics.Trace.TraceWarning(s);
            Assert.Equal('\0', options.CharP);
        }



    }


}
