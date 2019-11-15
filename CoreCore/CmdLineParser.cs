using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Fonlow.CommandLine
{
    /// <summary>
    /// This class shares a subset of the interfaces of Plossum's CommandLineParser, as a replacement.
    /// </summary>
    public class CommandLineParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options">Object decorated by Plossum's attributes.</param>
        public CommandLineParser(object options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            Options = options;

            CommandLineManagerAttribute = PlossumAttributesHelper.GetCommandLineManagerAttribute(options.GetType());

            OptionAttributes = PlossumAttributesHelper.GetCommandLineOptionAttributes(options.GetType());

            GroupAttributesDic = PlossumAttributesHelper.GetCommandLineOptionGroupAttributesDic(options.GetType());
        }

        internal object Options { get; private set; }

        internal Plossum.CommandLine.CommandLineOptionAttribute[] OptionAttributes { get; private set; }

        /// <summary>
        /// Mapping between GroupId and GroupAttribute.
        /// </summary>
        internal Dictionary<string, Plossum.CommandLine.CommandLineOptionGroupAttribute> GroupAttributesDic { get; private set; }

        UsageInfo usageInfo;

        /// <summary>
        /// Gets an object containing the descriptive properties of the option manager from which this instance was
        /// created.
        /// </summary>
        /// <value>The usage description.</value>
        /// <remarks>The returned <see cref="UsageInfo"/> can be used to change descriptions of the options
        /// and groups of this parser, and to retrieve a formatted string suitable for displaying as a help 
        /// message to the user.</remarks>
        public UsageInfo UsageInfo
        {
            get
            {
                if (usageInfo == null)
                {
                    usageInfo = new UsageInfo(this);
                }
                return usageInfo;
            }
        }

        /// <summary>
        /// Performs the actual parsing of the command line, setting (or calling) the relevant members of the
        /// option manager.
        /// </summary>
        /// <remarks><para>The command line from <see cref="Environment.CommandLine"/> will be used for parsing, assuming
        /// that the first argument is the path to the executable of this application.</para>
        /// <para>After parsing has been completed the <see cref="HasErrors"/> property and <see cref="ErrorMessage"/> property
        /// should be examined to determine if the command line was correct or not. If there are errors, the state
        /// of the option manager object (i.e. which properties has been set or not) is undefined.</para></remarks>
        public void Parse()
        {
            Parse(Environment.CommandLine, true);
        }

        /// <summary>
        /// Performs the actual parsing of a command line, setting (or calling) the relevant members of the
        /// option manager.
        /// </summary>
        /// <param name="commandLine">A string containing the command line passed to be parsed.</param>
        /// <param name="containsExecutable">if set to <c>true</c> it is assumed that the
        /// first argument on the command line is the path to the executable used to run this program.</param>
        /// <remarks>After parsing has been completed the <see cref="HasErrors"/> property and <see cref="ErrorMessage"/> property
        /// should be examined to determine if the command line was correct or not. If there are errors, the state
        /// of the option manager object (i.e. which properties has been set or not) is undefined.</remarks>
        public void Parse(string commandLine, bool containsExecutable)
        {
            if (String.IsNullOrWhiteSpace(commandLine))
                throw new ArgumentNullException("commandLine");

            if (containsExecutable)
            {
                string command, argsText;
                if (SplitCommandLine(commandLine, out command, out argsText))
                {
                    ErrorMessage = ParseAndAnalyze(argsText, Options);
                    ExecutablePath = command;
                }
                else
                {
                    ErrorMessage = String.Format("Commandline {0} looks very strange.", commandLine);
                }
            }
            else
            {
                ErrorMessage = ParseAndAnalyze(commandLine, Options);
            }
        }

        /// <summary>
        /// Performs the actual parsing of a command line, setting (or calling) the relevant members of the
        /// option manager.
        /// </summary>
        /// <param name="commandLine">A string containing the command line passed to be parsed. It is assumed that the
        /// first argument on the command line is the path to the executable used to run this program.</param>
        /// <remarks>After parsing has been completed the <see cref="HasErrors"/> property and <see cref="ErrorMessage"/> property
        /// should be examined to determine if the command line was correct or not. If there are errors, the state
        /// of the option manager object (i.e. which properties has been set or not) is undefined.</remarks>
        public void Parse(string commandLine)
        {
            Parse(commandLine, true);
        }

        /// <summary>
        /// Performs the actual parsing of a command line, setting (or calling) the relevant members of the
        /// option manager.
        /// </summary>
        /// <param name="args">An array containing the arguments passed to this program.</param>
        /// <param name="containsExecutable">if set to <c>true</c> it is assumed that the
        /// first argument in this list is the path to the executable used to run this program.</param>
        /// <remarks>After parsing has been completed the <see cref="HasErrors"/> property and <see cref="ErrorMessage"/> property
        /// should be examined to determine if the command line was correct or not. If there are errors, the state
        /// of the option manager object (i.e. which properties has been set or not) is undefined.</remarks>
        public void Parse(string[] args, bool containsExecutable)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            if (containsExecutable)
            {
                string[] optionArgs = new string[args.Length - 1];
                args.CopyTo(optionArgs, 1);
                ErrorMessage = ParseAndAnalyze(optionArgs, Options);
            }
            else
            {
                ErrorMessage = ParseAndAnalyze(args, Options);
            }
        }


        /// <summary>
        /// Performs the actual parsing of a command line, setting (or calling) the relevant members of the
        /// option manager.
        /// </summary>
        /// <param name="args">An array containing the arguments passed to this program. It is assumed that the
        /// first argument in this list is the path to the executable used to run this program.</param>
        /// <remarks>After parsing has been completed the <see cref="HasErrors"/> property and <see cref="ErrorMessage"/> property
        /// should be examined to determine if the command line was correct or not. If there are errors, the state
        /// of the option manager object (i.e. which properties has been set or not) is undefined.</remarks>
        public void Parse(string[] args)
        {
            ErrorMessage = ParseAndAnalyze(args, Options);
        }

        static string ParseAndAnalyze(string commandLine, object options)
        {
            var parserErrorMessage = OptionsReader.ReadOptions(commandLine, options);
            var validationErrorMessage = OptionsValidator.AnalyzeAssignedOptions(options);
            if (String.IsNullOrEmpty(parserErrorMessage) && String.IsNullOrEmpty(validationErrorMessage))
                return null;

            return parserErrorMessage + Environment.NewLine + validationErrorMessage;
        }

        static string ParseAndAnalyze(string[] args, object options)
        {
            var parserErrorMessage = OptionsReader.ReadOptions(args, options);
            var validationErrorMessage = OptionsValidator.AnalyzeAssignedOptions(options);
            if (String.IsNullOrEmpty(parserErrorMessage) && String.IsNullOrEmpty(validationErrorMessage))
                return null;

            return parserErrorMessage + Environment.NewLine + validationErrorMessage;
        }

        static bool SplitCommandLine(string commandLine, out string command, out string argsText)
        {
            Regex regex = new Regex(CommandLineGui.Properties.Resources.filePathPattern, RegexOptions.IgnorePatternWhitespace);
            var match = regex.Match(commandLine);
            if (match.Success)
            {
                command = match.Value;
                argsText = commandLine.Substring(match.Length);
                return true;
            }

            command = null;
            argsText = null;
            return false;
        }

        /// <summary>
        /// Error message in one or multiple lines about parsing and anlaysing.
        /// </summary>
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// True if there exists error message
        /// </summary>
        public bool HasErrors { get { return !String.IsNullOrEmpty(ErrorMessage); } }

        /// <summary>
        /// Gets the path to the executable of this application if it was included on the command line (it normally is), or null otherwise.
        /// </summary>
        /// <value>the path to the executable of this application if it was included on the command line.</value>
        /// <remarks>This should only be evaluated after <see cref="Parse()"/> has been called.</remarks>
        public string ExecutablePath { get; private set; }

        internal Plossum.CommandLine.CommandLineManagerAttribute CommandLineManagerAttribute { get; private set; }

        public string ApplicationName { get { return CommandLineManagerAttribute.ApplicationName; } }

        public string ApplicationDescription { get { return CommandLineManagerAttribute.Description; } }

        public string ApplicationCopyright { get { return CommandLineManagerAttribute.Copyright; } }

        public string ApplicationVersion { get { return CommandLineManagerAttribute.Version; } }
    }
}
