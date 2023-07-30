using System;
using System.Linq;
using Xunit;
using Fonlow.CommandLineGui;
using Fonlow.CommandLineGui.Robocopy;
using System.ComponentModel;
using Fonlow.CommandLine;

namespace TestBetter
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	 
	public class BasicTests
	{
		public BasicTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[Fact]
		public void TestStringToEnum()
		{
			CopyFlags flags = CopyFlags.A | CopyFlags.D | CopyFlags.O;
			Assert.Equal("DAO", TypeDescriptor.GetConverter(typeof(CopyFlags)).ConvertToString(flags));

			Rashcneto ff = Rashcneto.R | Rashcneto.A | Rashcneto.S | Rashcneto.H;
			Assert.Equal("RASH", TypeDescriptor.GetConverter(typeof(Rashcneto)).ConvertToString(ff));
		}

		[Fact]
		public void TestEnumToString()
		{
			CopyFlags flags = CopyFlags.A | CopyFlags.D | CopyFlags.O;
			Assert.Equal((int)flags, TypeDescriptor.GetConverter(typeof(CopyFlags)).ConvertFromString("DAO"));

			Rashcneto ff = Rashcneto.R | Rashcneto.A | Rashcneto.S | Rashcneto.H;
			Assert.Equal((int)ff, TypeDescriptor.GetConverter(typeof(Rashcneto)).ConvertFromString("RASH"));

		}

		//[Fact] https://stackoverflow.com/questions/59678504/how-to-unit-test-private-method-in-net-core-application-which-does-not-support
		//public void TestAddTemplate()
		//{
		//    CommandsTemplates.Instance.AddTemplate("FirstCommand", "DoSomething", "FirstCommand /ok", false);
		//    CommandsTemplates.Instance.AddTemplate("FirstCommand", "DoAnotherThing", "FirstCommand /haha", false);
		//    CommandsTemplates.Instance.AddTemplate("SecondCommand", "DoSomething", "SecondCommand /ok", false);
		//    var templatesNames = CommandsTemplates.Instance.GetTemplatesNames("FirstCommand");
		//    Assert.Equal(2, templatesNames.Count());

		//    CommandsTemplatesFile.Save(CommandsTemplates.Instance);

		//    CommandsTemplates.Instance.DeleteTemplate("FirstCommand", "DoAnotherThing");
		//    templatesNames = CommandsTemplates.Instance.GetTemplatesNames("FirstCommand");
		//    Assert.Equal(1, templatesNames.Count());

		//    var ct = new PrivateType(typeof(CommandsTemplatesFile));
		//    var dic = ct.InvokeStatic("Load") as CommandsTemplatesDictionary;//Singleton of CommandsTemplates is broken here, but this is fine in unit testing.
		//    Assert.Equal(2, dic.Count);

		//}

		[Fact]
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
			Assert.Equal(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(4, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
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
			Assert.Equal(5, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMaxOccursOK()
		{
			var options = new MaxOccursOptions
			{
				Files = new string[] { "abc", "efg" },
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(0, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMaxOccursSilly()
		{
			var options = new MaxOccursOptionsSilly
			{
				Silly="kk"
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMaxOccurs()
		{
			var options = new MaxOccursOptions
			{
				Files = new string[] { "abc", "efg", "hkkjk", "444" },
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMinOccursExpect2But1()
		{
			var options = new MaxOccursOptions
			{
				Files = new string[] { "abc" },
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMinOccursExpect2But0()
		{
			var options = new MaxOccursOptions
			{
				Files = new string[] { },
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestMinOccursExpect2ButNull()
		{
			var options = new MaxOccursOptions();

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestNumericOptionsWithGoodValue()
		{
			var options = new NumericOptions
			{
				Money = 100
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(0, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestNumericOptionsWithTooSmallValue()
		{
			var options = new NumericOptions
			{
				Money = 5
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestNumericOptionsWithTooGreatValue()
		{
			var options = new NumericOptions
			{
				Money = 100000
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestNumericOptionsWithArray()
		{
			var options = new NumericOptions
			{
				Prices = new int[] { 5, 100, 300, 5000, 100000}
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}




		[Fact]
		public void TestDecimalOptionsWithTooSmallValue()
		{
			var options = new DecimalOptions
			{
				Money = 5
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestDecimalOptionsWithTooGreatValue()
		{
			var options = new DecimalOptions
			{
				Money = 100000
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(1, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}

		[Fact]
		public void TestDecimalOptionsWithArray()
		{
			var options = new DecimalOptions
			{
				Prices = new decimal[] { 5, 100, 300, 5000, 100000 }
			};

			var s = OptionsValidator.AnalyzeAssignedOptions(options);
			System.Diagnostics.Debug.WriteLine(s);
			Assert.Equal(2, s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Length);
		}



	}


}
