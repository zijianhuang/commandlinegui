using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fonlow.CommandLine.Antlr;
using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Fonlow.CommandLine;

namespace TestAntlr
{
    [TestClass]
    public class ParmetersParsingTests
    {
        [TestMethod]
        public void TestBasic()
        {
            var dic = ArgumentParserResult.Parse("/abc  sdfd /efg  \"This is some\" ok").OptionsDictionary;
            Assert.AreEqual(2, dic.Count);
            Assert.AreEqual("abc", dic.First().Key);
            Assert.AreEqual(1, dic["abc"].Length);
            Assert.AreEqual(2, dic["efg"].Length);
            Assert.AreEqual("This is some", dic["efg"][0]);
        }

        [TestMethod]
        public void TestMultipleInstancesOfTheSameParameter()
        {
            var dic = ArgumentParserResult.Parse("/abc=sdfd /abc  \"This is some\" ok").OptionsDictionary;
            Assert.AreEqual(3, dic["abc"].Length);
            Assert.AreEqual("sdfd", dic["abc"][0]);
            Assert.AreEqual("ok", dic["abc"][2]);
        }

        [TestMethod]
        public void TestRobocopy()
        {
            var dic = ArgumentParserResult.Parse("/s /lev:32 /copy:astou  /a+:RASH  /a-:CU  /256 /rh:1233-2356 /xf thisfile.txt \"c:\\test\\this dir\\something.txt\" /save:jobname").OptionsDictionary;
            Assert.AreEqual(9, dic.Count);
            Assert.AreEqual("s", dic.First().Key);
            Assert.AreEqual(0, dic["s"].Length);
            Assert.AreEqual(2, dic["xf"].Length);
            Assert.AreEqual("32", dic["lev"][0]);
            Assert.AreEqual("astou", dic["copy"][0]);
            Assert.AreEqual("RASH", dic["a+"][0]);
            Assert.AreEqual("CU", dic["a-"][0]);
            Assert.AreEqual(0, dic["256"].Length);
            Assert.AreEqual("1233-2356", dic["rh"][0]);
            Assert.AreEqual("thisfile.txt", dic["xf"][0]);
            Assert.AreEqual("c:\\test\\this dir\\something.txt", dic["xf"][1]);
            Assert.AreEqual("jobname", dic["save"][0]);
        }

        [TestMethod]
        public void TestSvcUtil()
        {
            var dic = ArgumentParserResult.Parse("/namespace:http://schemas.datacontract.org/2004/07/WcfService19,MyNamespace").OptionsDictionary;
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual("namespace", dic.First().Key);
            Assert.AreEqual("http://schemas.datacontract.org/2004/07/WcfService19,MyNamespace", dic["namespace"][0]);
        }

        [TestMethod]
        public void TestMakeCert()
        {
            var dic = ArgumentParserResult.Parse("-sk keyname -# 1234 -$ commercial -? -! -b 12/11/2014 -eku 123.456.789, 2244.2343.3 , 348.343 -nscp -sky exchange -sp \"Microsoft RSA SChannel Cryptographic Provider\" -sy 12 -n \"CN=XXZZYY\"").OptionsDictionary;
            Assert.AreEqual(12, dic.Count);
            Assert.AreEqual("sk", dic.First().Key);
            Assert.AreEqual(0, dic["?"].Length);
            Assert.AreEqual(0, dic["!"].Length);
            Assert.AreEqual("12/11/2014", dic["b"][0]);
            Assert.AreEqual("123.456.789,", dic["eku"][0]);
            Assert.AreEqual("Microsoft RSA SChannel Cryptographic Provider", dic["sp"][0]);
            Assert.AreEqual("12", dic["sy"][0]);
            Assert.AreEqual("CN=XXZZYY", dic["n"][0]);

        }

        [TestMethod]
        public void TestMsBuild()
        {
            var dic = ArgumentParserResult.Parse("/p:Configuration=Release /p:X=Y ").OptionsDictionary;
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(2, dic["p"].Length);
            Assert.AreEqual("Configuration=Release", dic["p"][0]);

        }

        [TestMethod]
        public void TestFixedParameters()
        {
            var result = ArgumentParserResult.Parse("abc efg   \"this file is ok\"   /p:Configuration=Release /p:X=Y ");
            var dic = result.OptionsDictionary;
            Assert.AreEqual(1, dic.Count);
            Assert.AreEqual(2, dic["p"].Length);
            Assert.AreEqual("Configuration=Release", dic["p"][0]);
            var fixedParameters = result.FixedParameters.ToArray();
            Assert.AreEqual(3, fixedParameters.Length);
            Assert.AreEqual("abc", fixedParameters[0]);
            Assert.AreEqual("this file is ok", fixedParameters[2]);
        }

        [TestMethod]
        public void TestSilly()
        {
            var dic = ArgumentParserResult.Parse("abcde dsfdfd sdf").OptionsDictionary;
            Assert.AreEqual(0, dic.Count);

        }    
    
    

    
    } 


}
