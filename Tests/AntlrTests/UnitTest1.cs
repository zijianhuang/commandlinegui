using Fonlow.CommandLine;

namespace TestAntlr
{

	public class ParmetersParsingTests
	{
		[Fact]
		public void TestBasic()
		{
			var dic = ArgumentParserResult.Parse("/abc  sdfd /efg  \"This is some\" ok").OptionsDictionary;
			Assert.Equal(2, dic.Count);
			Assert.Equal("abc", dic.First().Key);
			Assert.Single(dic["abc"]);
			Assert.Equal(2, dic["efg"].Length);
			Assert.Equal("This is some", dic["efg"][0]);
		}

		[Fact]
		public void TestMultipleInstancesOfTheSameParameter()
		{
			var dic = ArgumentParserResult.Parse("/abc=sdfd /abc  \"This is some\" ok").OptionsDictionary;
			Assert.Equal(3, dic["abc"].Length);
			Assert.Equal("sdfd", dic["abc"][0]);
			Assert.Equal("ok", dic["abc"][2]);
		}

		[Fact]
		public void TestRobocopy()
		{
			var dic = ArgumentParserResult.Parse("/s /lev:32 /copy:astou  /a+:RASH  /a-:CU  /256 /rh:1233-2356 /xf thisfile.txt \"c:\\test\\this dir\\something.txt\" /save:jobname").OptionsDictionary;
			Assert.Equal(9, dic.Count);
			Assert.Equal("s", dic.First().Key);
			Assert.Empty(dic["s"]);
			Assert.Equal(2, dic["xf"].Length);
			Assert.Equal("32", dic["lev"][0]);
			Assert.Equal("astou", dic["copy"][0]);
			Assert.Equal("RASH", dic["a+"][0]);
			Assert.Equal("CU", dic["a-"][0]);
			Assert.Empty(dic["256"]);
			Assert.Equal("1233-2356", dic["rh"][0]);
			Assert.Equal("thisfile.txt", dic["xf"][0]);
			Assert.Equal("c:\\test\\this dir\\something.txt", dic["xf"][1]);
			Assert.Equal("jobname", dic["save"][0]);
		}

		[Fact]
		public void TestSvcUtil()
		{
			var dic = ArgumentParserResult.Parse("/namespace:http://schemas.datacontract.org/2004/07/WcfService19,MyNamespace").OptionsDictionary;
			Assert.Single(dic);
			Assert.Equal("namespace", dic.First().Key);
			Assert.Equal("http://schemas.datacontract.org/2004/07/WcfService19,MyNamespace", dic["namespace"][0]);
		}

		[Fact]
		public void TestMakeCert()
		{
			var dic = ArgumentParserResult.Parse("-sk keyname -# 1234 -$ commercial -? -! -b 12/11/2014 -eku 123.456.789, 2244.2343.3 , 348.343 -nscp -sky exchange -sp \"Microsoft RSA SChannel Cryptographic Provider\" -sy 12 -n \"CN=XXZZYY\"").OptionsDictionary;
			Assert.Equal(12, dic.Count);
			Assert.Equal("sk", dic.First().Key);
			Assert.Empty(dic["?"]);
			Assert.Empty(dic["!"]);
			Assert.Equal("12/11/2014", dic["b"][0]);
			Assert.Equal("123.456.789,", dic["eku"][0]);
			Assert.Equal("Microsoft RSA SChannel Cryptographic Provider", dic["sp"][0]);
			Assert.Equal("12", dic["sy"][0]);
			Assert.Equal("CN=XXZZYY", dic["n"][0]);

		}

		[Fact]
		public void TestMsBuild()
		{
			var dic = ArgumentParserResult.Parse("/p:Configuration=Release /p:X=Y ").OptionsDictionary;
			Assert.Single(dic);
			Assert.Equal(2, dic["p"].Length);
			Assert.Equal("Configuration=Release", dic["p"][0]);

		}

		[Fact]
		public void TestFixedParameters()
		{
			var result = ArgumentParserResult.Parse("abc efg   \"this file is ok\"   /p:Configuration=Release /p:X=Y ");
			var dic = result.OptionsDictionary;
			Assert.Single(dic);
			Assert.Equal(2, dic["p"].Length);
			Assert.Equal("Configuration=Release", dic["p"][0]);
			var fixedParameters = result.FixedParameters.ToArray();
			Assert.Equal(3, fixedParameters.Length);
			Assert.Equal("abc", fixedParameters[0]);
			Assert.Equal("this file is ok", fixedParameters[2]);
		}

		[Fact]
		public void TestSilly()
		{
			var dic = ArgumentParserResult.Parse("abcde dsfdfd sdf").OptionsDictionary;
			Assert.Empty(dic);

		}    
	
	

	
	} 


}
