using System;
using Plossum.CommandLine;
using Fonlow.CommandLine;

namespace MyPlossum
{
    /// <summary>
    /// A simple demo program of using Plossum CommandLine Library.
    /// For more details, please check http://www.codeproject.com/Articles/19869/Powerful-and-simple-command-line-parsing-in-C
    /// However, in v3 of Command Line GUI, Plossum library was basically rewritten, and over 95 % of the original codes had gone. The runtime behavior, interfaces and semantic are slightly different.
    /// And this program could be used by CommandLineGui as an assembly, and AssemblyNameOfCommand in CommandLineGui.exe.config should define the assembly name,
    /// and make sure the name is MyPlossum, not MyPlossum.exe.
    /// </summary>
    /// <remarks>The Ploosum codes used in this CommandLineGui is a fragment of Plossum 0.5. 
    /// which contains some minor fixes against bugs and defects.</remarks>
    class Program
    {
        static int Main(string[] args)
        {
            var options = new Options();
            var parser = new CommandLineParser(options);
              Console.WriteLine(parser.ApplicationDescription);

            parser.Parse();
            if (parser.HasErrors)
            {
                System.Diagnostics.Trace.TraceWarning(parser.ErrorMessage);
                Console.WriteLine(parser.UsageInfo.GetOptionsAsString());
                return 1;
            }

           
            if (options.Help)
            {
                Console.WriteLine(parser.UsageInfo.ToString());
               // Console.ReadLine();
                return 0;
            }

            if (options.Filters != null)
            {
                Console.WriteLine("Filters: " + String.Join(", ", options.Filters));
            }
            

            return 0;
        }
    }

    [CommandLineManager(ApplicationName = "MyPlossum", Description = "Demostrate the power of Plossum", OptionSeparator="/", Assignment=":", Copyright="Fonlow (c) 2013", Version="1.1")]
    [CommandLineOptionGroup("detail", Name = "Detail")]
    [CommandLineOptionGroup("other", Name = "Other")]
    public class Options
    {
        public Options()
        {
        }
        [CommandLineOption(Aliases = "F", Description = "Function name, e.g., /F=FirstFunction")]
        public string Function { get; set; }

        [CommandLineOption(Description = "URL. e.g., /Url=https://sourceforge.net/projects/commandlinegui",  GroupId = "detail")]
        public string Url { get; set; }


        [CommandLineOption(Aliases = "du", Name = "Duration", Description = "Duration in second.", RequireExplicitAssignment = true, DefaultAssignmentValue = 3600.08d, GroupId = "detail")]
        public double DurationInSecond { get; set; }

        [CommandLineOption(Aliases = "FI,fil", Name = "Filters", Description = "Filters is an array that could be multiple values following the parameter name, or multiple instances of options sharing the same name each with one or multiple values"
            , RequireExplicitAssignment = false,  GroupId = "detail")]
        public string[] Filters { get; set; }

        [CommandLineOption(Aliases = "h ?", Name="Help", Description = "Shows this help text", GroupId = "other")]
        public bool Help
        {
            get;
            set;
        }

        [CommandLineOption(Aliases = "OE", Description = "enum text", GroupId = "detail")]
        public MyEnum OkEnum
        {
            get;
            set;
        }

    }

    public enum MyEnum { None, Hello, World, Plossum };

    ///// <summary>
    ///// MyPlossum needs only options.
    ///// </summary>
    //public class MyCommand : Fonlow.CommandLineGui.Command
    //{
    //    public MyCommand() : base(typeof(Options))
    //    {

    //    }
    //}
}
