using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using Plossum.CommandLine;
using System.Linq;
using Fonlow.CommandLine;

namespace Fonlow.CommandLineGui
{
    public sealed class CommandFactory
    {
        CommandFactory()
        {

        }

        /// <summary>
        /// Load the first ICommand type found in the assembly and instantiate it.
        /// </summary>
        /// <param name="assemblyName">The assembly must have a concrete class derived from ICommand, and generally from CommandBase, CommandWithOptions or CommandWithParametersAndOptions;
        /// and the class must have a constructor without parameter that calls a base constructor with proper options type and parameters type.</param>
        /// <returns>ICommand object. Null if not found</returns>
        public static ICommand CreateCommandFromAssembly(string assemblyName)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.Load(assemblyName);
                AppTraceSource.Instance.TraceInformation("Assembly {0} is loaded for type {1}.", assemblyName, "ICommand");
            }
            catch (System.IO.FileLoadException e)
            {
                AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, errors occur: {1}", assemblyName, e.Message));
                return null;
            }
            catch (BadImageFormatException e)
            {
                AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, errors occur: {1}", assemblyName, e.Message));
                //when file is a win32 dll.
                return null;
            }
            catch (System.IO.FileNotFoundException e)
            {
                AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, errors occur: {1}", assemblyName, e.Message));
                return null;
            }
            catch (ArgumentException e)
            {
                AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, errors occur: {1}", assemblyName, e.Message));
                return null;
            }


            try
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if ((type.IsClass) && (PropertyHelper.ReadAttribute<CommandLineManagerAttribute>(type)!=null))
                    {
                        ICommand command = (ICommand)Activator.CreateInstance(typeof(Command), type);
                        if (command != null)
                        {
                            return command;
                        }
                    }
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (Exception ex in e.LoaderExceptions)
                {
                    AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, GetTypes errors occur: {1}", assemblyName, ex.Message));
                }
            }
            catch (TargetInvocationException e)
            {
                AppTraceSource.Instance.TraceWarning(String.Format("When loading {0}, GetTypes errors occur: {1}", assemblyName, e.Message + "~~" + e.InnerException.Message));
            }

            return null;
        }
    }

    /// <summary>
    /// Base class of command line program that also needs GUI.
    /// </summary>
    public class Command : ICommand
    {

        /// <summary>
        /// Hold the proxy object of type dymamically created for PropertyGrid.
        /// </summary>
        public object ParametersAndOptionsProxy
        {
            get;
            private set;
        }

        public Command(Type typeOfCommandOptions)
        {
            originalTypeOfCommandOptions = typeOfCommandOptions;
            Type proxyTypeOfParametersAndOptions = CommandTypeBuilder.CreateProxyType(typeOfCommandOptions);

            ParametersAndOptionsProxy = Activator.CreateInstance(proxyTypeOfParametersAndOptions);
            propertiesOfParametersAndOptions = new List<PropertyInfo>(proxyTypeOfParametersAndOptions.GetProperties());
            propertiesOfParametersAndOptions.Sort(ComparePropertyOrder);

            commandLineManagerAttribute = PlossumAttributesHelper.GetCommandLineManagerAttribute(typeOfCommandOptions);

            ResetParameters();

            assignment = commandLineManagerAttribute.Assignment;

            const string template = @"\s*{0}(?<OptionName>\w*[\+|\-]?)";
            optionSeparator = commandLineManagerAttribute.OptionSeparator;
            optionNamePattern = String.Format(template, optionSeparator);

            allGroupAttributesDic = PlossumAttributesHelper.GetCommandLineOptionGroupAttributesDic(proxyTypeOfParametersAndOptions);

            Parser = new CommandLineParser(ParametersAndOptionsProxy);

        }

        Type originalTypeOfCommandOptions;

        CommandLineParser Parser;

        string optionNamePattern;

        string optionSeparator;

        CommandLineManagerAttribute commandLineManagerAttribute;

        Dictionary<string, CommandLineOptionGroupAttribute> allGroupAttributesDic;

        static int ComparePropertyOrder(PropertyInfo px, PropertyInfo py)
        {
            int orderx = PropertyHelper.GetParameterOrder(px);
            int ordery = PropertyHelper.GetParameterOrder(py);
            return orderx - ordery;
        }

        string assignment;


        /// <summary>
        /// Sorted properties
        /// </summary>
        List<PropertyInfo> propertiesOfParametersAndOptions;

        /// <summary>
        /// Parse command line including exe.
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns></returns>
        public virtual bool ParseCommandLine(string commandLine)
        {
            return ParseCommandLine(commandLine, true);
        }

        protected virtual bool ParseCommandLine(string commandLine, bool includeExe)
        {
            ResetParameters();//need to reset, since the parser only alter those options in command line.

            Parser.Parse(commandLine, includeExe);
            ValidateProxy();
            this.ExecutablePath = Parser.ExecutablePath;
            if (Parser.HasErrors)
            {
                return false;
            }


            return true;


        }

        /// <summary>
        /// Command name is given by commandLineManagerAttribute.ApplicationName.
        /// </summary>
        public string CommandName
        {
            get
            {
                return commandLineManagerAttribute.ApplicationName;
            }
        }

        protected string ExecutablePath { get; set; }

        /// <summary>
        /// According the position of cursor (SelectionPosition), identify the parameter highlighted.
        /// While this CommandBase returns null, the derived class should optionally override this function
        /// to provide specific analysis in order to pick up the parameter.
        /// Thus when a user highlight a parameter in the command text, the program may be able
        /// to locate a hint for the parameter.
        /// </summary>
        public virtual string PickupParameterAtPosition(string commandText, int position)
        {
            if (commandText == null)
                throw new ArgumentNullException("commandText");

            int prefixLen = optionSeparator.Length;

            int optionPrefixPosition = -1;
            for (int i = position; i > 0; i--)
            {
                if (commandText.Substring(i, prefixLen) == optionSeparator)
                {
                    optionPrefixPosition = i;
                    break;
                }
            }

            if (optionPrefixPosition > -1)
            {
                Regex parametersRegex = new Regex(optionNamePattern, RegexOptions.IgnoreCase);
                Match m = parametersRegex.Match(commandText.Substring(optionPrefixPosition));
                if (m.Success)
                {
                    if (m.Groups["OptionName"].Success)
                    {
                        string parameterName = m.Groups["OptionName"].Value;
                        return parameterName;
                    }
                }

            }
            return null;
        }

        /// <summary>
        /// Full command line with command, fixed parameters and options.
        /// </summary>
        public string CommandLine
        {
            get
            {
                string s = String.IsNullOrEmpty(ExecutablePath) ? CommandName : ExecutablePath;
                return s + " " + DefinedParametersAndOptions;
            }
        }

        /// <summary>
        /// Get what 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public string GetParameterDescription(string parameterName)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }

            foreach (PropertyInfo propertyInfo in propertiesOfParametersAndOptions)
            {
                if (parameterName.Equals(PropertyHelper.GetDisplayName(propertyInfo), StringComparison.CurrentCultureIgnoreCase))
                {
                    return PropertyHelper.GetPropertyDescription(propertyInfo);
                }

            }

            return null;
        }


        /// <summary>
        /// Set all values of DynamicParametersAndOptions to default values.
        /// </summary>
        void ResetParameters()
        {
            foreach (PropertyInfo propertyInfo in propertiesOfParametersAndOptions)
            {
                try
                {
                    var defaultValue = PropertyHelper.ReadDefaultValue(propertyInfo);
                    propertyInfo.SetValue(ParametersAndOptionsProxy, defaultValue, null);
                }
                catch (ArgumentException e)
                {
                    AppTraceSource.Instance.TraceWarning("When ResetParameters, for {0}, there is error: {1}", propertyInfo.Name, e.Message);
                }
                catch (TargetInvocationException e)
                {
                    AppTraceSource.Instance.TraceWarning("When ResetParameters, for {0}, there is error: {1}", propertyInfo.Name, e.ToString());
                }
            }

        }


        static string ConvertArrayToString(object array)
        {
            var methodInfo = array.GetType().GetMethod("ToString");
            var list = array as IEnumerable<object>;
            var textList = list.Select(d => PropertyHelper.SimplyQuoteString(((string)methodInfo.Invoke(d, null)).Trim()));
            return String.Join(" ", textList);
        }

        /// <summary>
        /// Text representing parameters and options with assigned values not equal to the default.
        /// </summary>
        public string DefinedParametersAndOptions
        {
            get
            {
                ValidateProxy();

                StringBuilder builder = new StringBuilder();
                foreach (PropertyInfo propertyInfo in propertiesOfParametersAndOptions)
                {
                    try
                    {
                        var propertyValue = propertyInfo.GetValue(ParametersAndOptionsProxy, null);
                        if (propertyValue == null)
                        {
                            continue;
                        }

                        int parameterOrder = PropertyHelper.GetParameterOrder(propertyInfo);
                        if (parameterOrder < int.MaxValue)//so this is a fixed parameter
                        {
                            if (propertyInfo.PropertyType == typeof(string))
                            {
                                builder.Append(PropertyHelper.SimplyQuoteString((string)propertyValue) + " ");
                            }
                            else if (propertyInfo.PropertyType.IsArray)
                            {
                                builder.Append(ConvertArrayToString(propertyValue) + " ");
                            }
                            else
                            {
                                builder.Append(propertyValue.ToString() + " ");
                            }
                        }
                        else//option
                        {
                            var defaultValue = PropertyHelper.ReadDefaultValue(propertyInfo);

                            var optionAttribute = PlossumAttributesHelper.GetCommandLineOptionAttribute(propertyInfo);
                            CommandLineOptionGroupAttribute optionGroupAttribute = null;
                            if (!String.IsNullOrWhiteSpace(optionAttribute.GroupId))
                            {
                                allGroupAttributesDic.TryGetValue(optionAttribute.GroupId, out optionGroupAttribute);
                            }

                            bool toHaveAssign = NeedAssignment(commandLineManagerAttribute, optionGroupAttribute, optionAttribute);

                            if (!propertyInfo.PropertyType.IsEnum && !propertyValue.Equals(defaultValue)) //handle non enum
                            {
                                if (propertyInfo.PropertyType == typeof(bool))
                                {
                                    builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo) + " ");
                                }
                                else
                                {
                                    if (propertyInfo.PropertyType == typeof(string))
                                    {
                                        builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo)
                                            + (toHaveAssign ? assignment : " ")
                                            + PropertyHelper.SimplyQuoteString((string)propertyValue) + " ");
                                    }
                                    else if (propertyInfo.PropertyType.IsArray)
                                    {
                                        builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo)
                                            + (toHaveAssign ? assignment : " ")
                                            + ConvertArrayToString(propertyValue) + " ");
                                    }
                                    else
                                    {
                                        builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo) + assignment + propertyValue + " ");
                                    }
                                }
                            }
                            else if ((propertyInfo.PropertyType.IsEnum) && ((int)propertyValue != (int)defaultValue))//Handle enum.
                            {
                                var converter = PropertyHelper.CreateTypeConverter(propertyInfo.PropertyType);
                                if (converter != null) // enum with custom converter
                                {
                                    builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo)
                                        + (toHaveAssign ? assignment : " ")
                                        + converter.ConvertToString(propertyValue) + " ");
                                }
                                else // enum with standard converter
                                {
                                    builder.Append(optionSeparator + PropertyHelper.GetDisplayName(propertyInfo)
                                        + (toHaveAssign ? assignment : " ")
                                        + propertyValue.ToString() + " ");
                                }
                            }

                        }

                    }
                    catch (ArgumentException e)
                    {
                        AppTraceSource.Instance.TraceWarning("When checking property values, for {0}, there is error: {1}", propertyInfo.Name, e.Message);
                    }
                }
                return builder.ToString();

            }
        }

        static bool NeedAssignment(CommandLineManagerAttribute managerAttribute, CommandLineOptionGroupAttribute groupAttribute, CommandLineOptionAttribute optionAttriubte)
        {
            if (optionAttriubte.RequireExplicitAssignmentAssigned)
                return optionAttriubte.RequireExplicitAssignment;
            else if (groupAttribute != null && groupAttribute.RequireExplicitAssignmentAssigned)
                return groupAttribute.RequireExplicitAssignment;
            else if (managerAttribute != null && managerAttribute.RequireExplicitAssignmentAssigned)
                return managerAttribute.RequireExplicitAssignment;

            return false;
        }

        void ValidateProxy()
        {
            var bufferObject = Activator.CreateInstance(originalTypeOfCommandOptions);
            var validator = bufferObject as IRefineOptions;
            if (validator == null)
                return;

            var mProperties = originalTypeOfCommandOptions.GetProperties();
            var pProperties = ParametersAndOptionsProxy.GetType().GetProperties();

            if (mProperties.Count() != pProperties.Count())
            {
                throw new InvalidOperationException("The program has serious problem. The numbers of properties don't match. ");
            }

            //copy from proxy to template
            for (int i = 0; i < pProperties.Length; i++)
            {
                var propertyInfo = pProperties[i];
                var value = propertyInfo.GetValue(ParametersAndOptionsProxy, null);
                var mp = mProperties[i];

                if (mp.Name == propertyInfo.Name)
                {
                    mp.SetValue(bufferObject, value, null);
                }
                else
                    throw new InvalidOperationException("The program has serious problem. Property names don't match. ");
            }

            try
            {
                validator.Validate();
                validator.Refine();

            }
            catch (InvalidParametersException e)
            {
                ReportError(e.Message);
                return;
            }
            //copy from template back to proxy
            for (int i = 0; i < mProperties.Length; i++)
            {
                var propertyInfo = mProperties[i];
                var value = propertyInfo.GetValue(bufferObject, null);
                var mp = pProperties[i];

                if (mp.Name == propertyInfo.Name)
                {
                    mp.SetValue(ParametersAndOptionsProxy, value, null);
                }
                else
                    throw new InvalidOperationException("The program has serious problem. Property names don't match. ");

            }
        }

        void ReportError(string s)
        {
            if (ReportErrorHandler != null)
            {
                ReportErrorHandler(this, new TextMessageEventArgs(s));
            }
        }

        /// <summary>
        /// To report critical critical error.
        /// </summary>
        public event EventHandler<TextMessageEventArgs> ReportErrorHandler;
    }



}
