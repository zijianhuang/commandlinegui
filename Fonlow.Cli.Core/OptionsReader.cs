using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace Fonlow.CommandLine
{
    /// <summary>
    /// Reader options text and put values to options.
    /// </summary>
    public static class OptionsReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">For options only. The caller should toss all fixed parameters out.</param>
        /// <param name="options"></param>
        /// <returns>Error message</returns>
        public static string ReadOptions(string[] args, object options)
        {
            if ((args==null) || (args.Length == 0))
                return null;

            if (options == null)
                throw new ArgumentNullException("options");

            var refined = args.Select(d => d.Contains(' ') ? ('"' + d + '"') : d);//in command line, program will strip off the double quotes so I need to reintroduce.
            var s = String.Join(" ", refined);
            Debug.WriteLine("refined: " + s);
            return ReadOptions(s, options);
        }

        /// <summary>
        /// Parse option text and assign property values to options accordingly.
        /// </summary>
        /// <param name="optionsText">For options only. The caller should toss all fixed parameters out.</param>
        /// <param name="options"></param>
        /// <returns>Error message</returns>
        public static string ReadOptions(string optionsText, object options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            if (String.IsNullOrWhiteSpace(optionsText))
                return null;

            var parserResult= ArgumentParserResult.Parse(optionsText);

            var fixedParameters = parserResult.FixedParameters;
            


            var optionsDic = parserResult.OptionsDictionary;
            if (optionsDic.Count == 0)
                return "Cannot find any token when parsing optionsText.";

            var optionsType = options.GetType();
            var propertiesOfOptions = optionsType.GetProperties();


            #region Handle fixed parameters

            List<PropertyInfo> propertiesOfFixedParameters = new List<PropertyInfo>();
            foreach (var propertyItem in propertiesOfOptions)
            {
                var orderAttribute = PropertyHelper.ReadAttribute<ParameterOrderAttribute>(propertyItem);
                if (orderAttribute != null)
                {
                    propertiesOfFixedParameters.Add(propertyItem);
                }
            }

            int upperBoundOfFixedParameters = fixedParameters.Count();
            int lastAssignedIndex = 0;
            for (int i = 0; i < propertiesOfFixedParameters.Count; i++)
            {
                var propertyInfo = propertiesOfFixedParameters[i];
                if ((i < upperBoundOfFixedParameters) && (!propertyInfo.PropertyType.IsArray))  // in case fixed parameters in command line is less then those properties in the object, so the rest won't be assigned.
                {
                    propertyInfo.SetValue(options, fixedParameters[i], null);
                    lastAssignedIndex=i;
                }
                else
                    break;
            }

            if (lastAssignedIndex < propertiesOfFixedParameters.Count - 1)
            {
                var lastIndex = lastAssignedIndex + 1;
                var propertyInfo = propertiesOfFixedParameters[lastIndex];
                if (propertyInfo.PropertyType.IsArray)
                {
                    var restParameters = fixedParameters.Skip(lastIndex).ToArray();
                    propertyInfo.SetValue(options, restParameters, null);
                }
            }

            #endregion


            StringBuilder builder = new StringBuilder();

            List<string> allParameterNames = new List<string>();

            foreach (var propertyItem in propertiesOfOptions)
            {
                var optionAttribute = PlossumAttributesHelper.GetCommandLineOptionAttribute(propertyItem);

                if (optionAttribute != null)
                {
                    string[] optionValues = null;
                    string optionName;
                    if (String.IsNullOrWhiteSpace(optionAttribute.Name))
                    {
                        optionName = propertyItem.Name;
                        optionAttribute.Name = optionName;
                    }
                    else
                    {
                        optionName = optionAttribute.Name;
                    }

                    allParameterNames.Add(optionName);

                    bool parameterNameFound = false;

                    Action tryGetValuesArrayFromDic = () =>
                    {
                        List<string> list = new List<string>();
                        string[] array;
                        if (optionsDic.TryGetValue(optionName, out array))
                        {
                            list.AddRange(array);
                            parameterNameFound = true;
                        }

                        if (optionAttribute.AliasesArray != null)
                        {
                            allParameterNames.AddRange(optionAttribute.AliasesArray);

                            foreach (var a in optionAttribute.AliasesArray)
                            {
                                if (optionsDic.TryGetValue(a, out array))
                                {
                                    list.AddRange(array);
                                    parameterNameFound = true;
                                }
                            }
                        }

                        optionValues = list.ToArray();
                    };

                    tryGetValuesArrayFromDic();


                    if (parameterNameFound)
                    {
                        if (propertyItem.PropertyType == typeof(bool)) //bool parameter has no explicit value defined
                        {
                            propertyItem.SetValue(options, true, null);
                            if (optionValues.Length > 0)
                            {
                                builder.AppendLine(String.Format("Boolean option {0} should not have explicit values.", optionName));
                            }
                            continue;
                        }

                        if (optionValues.Length == 0)
                        {
                            builder.AppendLine(String.Format("Option {0} expects some values but no value is found with this option.", optionAttribute.Name));
                            continue;
                        }

                        if (!propertyItem.PropertyType.IsArray)
                        {
                            var valueText = optionValues[0];

                            if (optionValues.Length > 1)
                            {
                                builder.Append(String.Format(String.Format("Option {0} of type {1} has more than one value assigned: {2}. However, the first value {3} is used."
                                    , optionName, propertyItem.PropertyType.ToString(), String.Join(", ", optionValues), valueText)));
                            }

                            if (propertyItem.PropertyType == typeof(string))
                            {
                                propertyItem.SetValue(options, valueText, null);
                                continue;
                            }

                            Action addError = () => { builder.AppendLine(String.Format("Option {0} of type {1} has some invalid value {2}", optionName, propertyItem.PropertyType.ToString(), valueText)); };

                            if (propertyItem.PropertyType == typeof(int))
                            {
                                int value;
                                if (!int.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(byte))
                            {
                                byte value;
                                if (!byte.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(sbyte))
                            {
                                sbyte value;
                                if (!sbyte.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(char))
                            {
                                char value;
                                if (!char.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(decimal))
                            {
                                decimal value;
                                if (!decimal.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(double))
                            {
                                double value;
                                if (!double.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(float))
                            {
                                float value;
                                if (!float.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(uint))
                            {
                                uint value;
                                if (!uint.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(long))
                            {
                                long value;
                                if (!long.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(ulong))
                            {
                                ulong value;
                                if (!ulong.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(short))
                            {
                                short value;
                                if (!short.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType == typeof(ushort))
                            {
                                ushort value;
                                if (!ushort.TryParse(valueText, out value))
                                    addError();
                                else
                                    propertyItem.SetValue(options, value, null);

                                continue;
                            }

                            if (propertyItem.PropertyType.IsEnum)
                            {
                                try
                                {
                                    var converter = PropertyHelper.CreateTypeConverter(propertyItem.PropertyType);

                                    object value = converter == null ? Enum.Parse(propertyItem.PropertyType, valueText, true) //starndard conversion
                                        : converter.ConvertFromString(valueText);//custom 
                                    propertyItem.SetValue(options, value, null);
                                }
                                catch (ArgumentException e)
                                {
                                    builder.AppendLine(String.Format("Option {0} has some invalid values {1}. {2}", optionAttribute.Name, optionValues, e.Message));
                                }
                                catch (OverflowException e)
                                {
                                    builder.AppendLine(String.Format("Option {0} has some values {1} out of range. {2}", optionAttribute.Name, optionValues, e.Message));
                                }


                                continue;
                            }

                        }
                        else //Handle values array of parameter
                        {
                            if (propertyItem.PropertyType == typeof(string[]))
                            {
                                propertyItem.SetValue(options, optionValues, null);
                                continue;
                            }


                            //bool array haven't been seen, not supported 

                            if (HandleValueArray<int>(int.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<byte>(byte.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<sbyte>(sbyte.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<decimal>(decimal.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<double>(double.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<float>(float.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<uint>(uint.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<long>(long.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<ulong>(ulong.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<short>(short.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                            if (HandleValueArray<ushort>(ushort.Parse, propertyItem, options, optionValues, optionAttribute, builder))
                                continue;

                        }


                    }
                    else
                    {
                        continue;
                    }
                }

            }


            var unknown = optionsDic.Keys.Where(d => ! allParameterNames.Any(p => p.Equals(d, StringComparison.CurrentCultureIgnoreCase))).ToArray();
            if (unknown.Length > 0)
            {
                builder.AppendLine("Unknown options: " + String.Join(" ", unknown));
            }

            return builder.ToString();
        }

        static bool HandleValueArray<T>(Func<string, T> parse, System.Reflection.PropertyInfo propertyItem, object options, string[] optionValues, Plossum.CommandLine.CommandLineOptionAttribute optionAttribute, StringBuilder builder)
        {
            if (propertyItem.PropertyType == typeof(T[]))
            {
                try
                {
                    T[] values = optionValues.Select(d => parse(d)).ToArray();
                    propertyItem.SetValue(options, values, null);

                }
                catch (FormatException e)
                {
                    builder.AppendLine(String.Format("Option {0} has some invalid values in ( {1} ). {2}", optionAttribute.Name, String.Join(", ", optionValues), e.Message));
                }
                catch (OverflowException e)
                {
                    builder.AppendLine(String.Format("Option {0} has some values in ( {1} ) out of range. {2}", optionAttribute.Name, String.Join(", ", optionValues), e.Message));
                }

                return true;
            }

            return false;
        }
    }
}
