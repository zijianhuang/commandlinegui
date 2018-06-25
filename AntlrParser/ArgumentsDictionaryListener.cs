using System;
using System.Collections.Generic;
using System.Diagnostics;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Fonlow.CommandLine.Antlr;

namespace Fonlow.CommandLine
{
    public class ArgumentParserResult
    {
         ArgumentParserResult(string optionsText)
        {
            ArgumentsLexer lexer = new ArgumentsLexer(new Antlr4.Runtime.AntlrInputStream(optionsText));
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            ArgumentsParser parser = new ArgumentsParser(tokens);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new TraceErrorListener());

            ParseTreeWalker walker = new ParseTreeWalker();
            listener = new ArgumentsDictionaryListener();
            walker.Walk(listener, parser.init());

        }

        public static ArgumentParserResult Parse(string optionsText)
        {
            return new ArgumentParserResult(optionsText);
        }

        ArgumentsDictionaryListener listener;

        /// <summary>
        /// Key to hold parameter name, and string array to hold values of the parameter.
        /// </summary>
        /// <param name="optionsText"></param>
        /// <returns></returns>
        public Dictionary<string, string[]> OptionsDictionary
        {
            get { return listener.OptionsDic; }
        }

        public IList<string> FixedParameters
        {
            get { return listener.FixedParameters; }
        }
    }

    /// <summary>
    /// Buffer parameters in a dictionary
    /// </summary>
    internal class ArgumentsDictionaryListener : ArgumentsBaseListener
    {
        public ArgumentsDictionaryListener()
        {
            optionsDic = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            fixedParameterValues = new List<string>();
        }


        string currentKey;

        List<string> currentValues;

        Dictionary<string, string[]> optionsDic;
        public Dictionary<string, string[]> OptionsDic { get { return optionsDic; } }

        List<string> fixedParameterValues;

        public IList<string> FixedParameters { get { return fixedParameterValues; } }

        public override void EnterParameter(ArgumentsParser.ParameterContext context)
        {
            currentValues = new List<string>();
        }

        public override void ExitParameter(ArgumentsParser.ParameterContext context)
        {
            string[] existingValues;
            if (currentKey == null)
            {
                Trace.TraceError("currentKey is null in ExitParameter");
                return;
            }
            if (optionsDic.TryGetValue(currentKey, out existingValues))//to support multiple instances of the same parameter
            {
                currentValues.InsertRange(0, existingValues);
                optionsDic[currentKey] = currentValues.ToArray();
            }
            else
            {
                optionsDic.Add(currentKey, currentValues.ToArray());
            }
        }

        public override void EnterParameterName(ArgumentsParser.ParameterNameContext context)
        {
            currentKey = context.GetText();
            Debug.WriteLine(currentKey);

        }

        public override void EnterValue(ArgumentsParser.ValueContext context)
        {
            var v = context.GetText();
            if (v.Length == 0)
                return; //Antlr should have emit warning already
            Debug.WriteLine(v);
            if (currentValues==null)
            {
                currentValues = new List<string>();
            }

            if (v[0] == '"')
            {
                currentValues.Add(v.Trim('"'));
            }
            else
            {
                currentValues.Add(v);
            }
        }

        public override void EnterFixedParameter(ArgumentsParser.FixedParameterContext context)
        {
            var v = context.GetText();
            if (v.Length == 0)
                return; //Antlr should have emit warning already
            Debug.WriteLine(v);
            if (v[0] == '"')
            {
                fixedParameterValues.Add(v.Trim('"'));
            }
            else
            {
                fixedParameterValues.Add(v);
            }
        }
    }

    internal class TraceErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            System.Diagnostics.Trace.TraceWarning("Token: {0} -- {1}", offendingSymbol.Text, msg);
        }
    }


}
