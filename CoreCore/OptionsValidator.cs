using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plossum.CommandLine;
using System.Reflection;
using System.Diagnostics;

namespace Fonlow.CommandLine
{
    public static class OptionsValidator
    {
        private static readonly char[] mSeparators = new char[] { ' ', ',', ';' };

        public static string AnalyzeAssignedOptions(object optionsAssigned)
        {
            if (optionsAssigned == null)
                throw new ArgumentNullException("optionsAssigned");

            var typeOfParametersAndOptions = optionsAssigned.GetType();
            var propertiesOfParametersAndOptions = new List<PropertyInfo>(typeOfParametersAndOptions.GetProperties());
            var managerAttribute = PlossumAttributesHelper.GetCommandLineManagerAttribute(typeOfParametersAndOptions);
            var isCaseSensitive = managerAttribute.IsCaseSensitive;

            var allOptionAttributes = PlossumAttributesHelper.GetCommandLineOptionAttributes(typeOfParametersAndOptions);
            var allPropertyInfo = PlossumAttributesHelper.GetOptionProperties(typeOfParametersAndOptions);

            List<PropertyInfo> propertyInfoOfAssignedProperties = new List<PropertyInfo>();
            List<CommandLineOptionAttribute> optionAttributesOfAssignedProperties = new List<CommandLineOptionAttribute>();

            foreach (PropertyInfo propertyInfo in propertiesOfParametersAndOptions)
            {
                var propertyValue = propertyInfo.GetValue(optionsAssigned, null);
                if (propertyValue == null)
                {
                    continue;
                }

                int parameterOrder = PropertyHelper.GetParameterOrder(propertyInfo);
                if (parameterOrder < int.MaxValue)//so this is a fixed parameter
                {
                    continue;
                }
                else//option
                {
                    var defaultValue = PropertyHelper.ReadDefaultValue(propertyInfo);
                    var optionAttribute = PlossumAttributesHelper.GetCommandLineOptionAttribute(propertyInfo);

                    if (!propertyValue.Equals(defaultValue))
                    {
                        optionAttributesOfAssignedProperties.Add(optionAttribute);
                        propertyInfoOfAssignedProperties.Add(propertyInfo);
                    }
                }
            }

            #region about Grouped options

            var optionAttributesGrouped = optionAttributesOfAssignedProperties.GroupBy(d => d.GroupId).OrderBy(k => k.Key);
            var allGroupAttributesDic = PlossumAttributesHelper.GetCommandLineOptionGroupAttributesDic(typeOfParametersAndOptions);

            StringBuilder builder = new StringBuilder();
            foreach (var item in allGroupAttributesDic)
            {
                var groupId = item.Key;
                var groupAttribute = item.Value;
                var optionAttributes = optionAttributesGrouped.FirstOrDefault(d => d.Key == groupId);

                switch (groupAttribute.Require)
                {
                    case OptionGroupRequirement.None:
                        continue;
                    case OptionGroupRequirement.AtMostOne:
                        if (optionAttributes == null)
                            continue;

                        if (!(optionAttributes.Count() <= 1))
                        {
                            builder.AppendLine(String.Format("Group {0} requires at most one option defined, but actually these are {1} options defined", groupAttribute.Name, optionAttributes.Count()));
                        }
                        break;
                    case OptionGroupRequirement.AtLeastOne:
                        if ((optionAttributes == null)
                            || !(optionAttributes.Count() >= 1))
                        {
                            builder.AppendLine(String.Format("Group {0} requires at least one option defined, but actually these is none defined", groupAttribute.Name));
                        }

                        break;
                    case OptionGroupRequirement.ExactlyOne:
                        if (optionAttributes == null)
                        {
                            builder.AppendLine(String.Format("Group {0} requires exactly one option defined, but actually these is none defined", groupAttribute.Name));

                        }
                        else if (!(optionAttributes.Count() == 1))
                        {
                            builder.AppendLine(String.Format("Group {0} requires exactly one option defined, but actually these are {1} options defined.", groupAttribute.Name, optionAttributes.Count()));
                        }

                        break;
                    case OptionGroupRequirement.All:
                        if (optionAttributes == null)
                        {
                            builder.AppendLine(String.Format("Group {0} requires all options defined, but actually these is none defined", groupAttribute.Name));
                        }
                        else
                        {
                            var numberOfOptionsOfGroup = allOptionAttributes.Count(d => d.GroupId == groupId);
                            var delta = numberOfOptionsOfGroup - optionAttributes.Count();
                            if (delta != 0)
                            {
                                builder.AppendLine(String.Format("Group {0} requires all options defined, but actually there are {1} options(s) missing.", groupAttribute.Name, delta));
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            #endregion

            #region about Prohibits


            for (int i = 0; i < propertyInfoOfAssignedProperties.Count; i++)
            {
                var optionAttribute = optionAttributesOfAssignedProperties[i];
                if (String.IsNullOrEmpty(optionAttribute.Prohibits))
                    continue;

                var propertyInfo = propertyInfoOfAssignedProperties[i];

                string[] optionNamesProhibited = optionAttribute.Prohibits.Split(mSeparators, StringSplitOptions.RemoveEmptyEntries);
                if (optionNamesProhibited.Length == 0)
                    continue;

                for (int k = 0; k < propertyInfoOfAssignedProperties.Count; k++)
                {
                    var nextPropertyInfo = propertyInfoOfAssignedProperties[k];
                    bool matched = false;
                    if (optionNamesProhibited.Contains(nextPropertyInfo.Name, isCaseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase))
                    {
                        matched = true;
                    }
                    else
                    {
                        var nextOptionAttribute = optionAttributesOfAssignedProperties[k];
                        if (optionNamesProhibited.Contains(nextOptionAttribute.Name, isCaseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase))
                        {
                            matched = true;
                        }
                        else
                        {
                            if ((nextOptionAttribute.AliasesArray != null) && (nextOptionAttribute.AliasesArray.Length > 0))
                            {
                                matched = optionNamesProhibited.Any(d => nextOptionAttribute.AliasesArray.Contains(d, isCaseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase));
                            }
                        }
                    }

                    if (matched)
                    {
                        builder.AppendLine(String.Format("Option {0} defined is prohibited since option {1} is already defined.", nextPropertyInfo.Name, propertyInfo.Name));
                    }
                }

            }


            #endregion


            for (int i = 0; i < propertyInfoOfAssignedProperties.Count; i++)
            {
                var optionAttribute = optionAttributesOfAssignedProperties[i];
                var propertyInfo = propertyInfoOfAssignedProperties[i];

                if (propertyInfo.PropertyType.IsArray)
                {
                    var array = propertyInfo.GetValue(optionsAssigned, null);
                    var list = array as Array;
                    var arrayLength = list.Length;
                    if ((optionAttribute.MaxOccurs > 1) && (arrayLength > optionAttribute.MaxOccurs))
                    {
                        builder.AppendLine(String.Format("Option {0} with MaxOccurs={1} has {2} presented.", propertyInfo.Name, optionAttribute.MaxOccurs, arrayLength));
                    }

                    if ((optionAttribute.MinOccurs > 0) && (arrayLength < optionAttribute.MinOccurs))
                    {
                        builder.AppendLine(String.Format("Option {0} with MinOccurs={1} has {2} presented.", propertyInfo.Name, optionAttribute.MinOccurs, arrayLength));
                    }

                    if ((optionAttribute.MaxOccurs > 0) && (optionAttribute.MinOccurs > optionAttribute.MaxOccurs))
                    {
                        builder.AppendLine(String.Format("In option {0} MinOccurs={1} is greater than MaxOccurs={2} .", propertyInfo.Name, optionAttribute.MinOccurs, optionAttribute.MaxOccurs));
                    }
                }
            }

            for (int i = 0; i < allPropertyInfo.Length; i++)
            {
                var optionAttribute = allOptionAttributes[i];
                var propertyInfo = allPropertyInfo[i];

                if (propertyInfo.PropertyType.IsArray)
                {
                    var array = propertyInfo.GetValue(optionsAssigned, null);
                    var list = array as Array;
                    //                var arrayLength = list.Length;
                    if ((list == null) && (optionAttribute.MinOccurs > 0))
                    {
                        builder.AppendLine(String.Format("Option {0} with MinOccurs={1} has no value.", propertyInfo.Name, optionAttribute.MinOccurs));
                    }
                }
                else
                {
                    if (optionAttribute.MaxOccurs > 1)
                    {
                        builder.AppendLine(String.Format("Option {0} must not have MaxOccurs greater than 1.", propertyInfo.Name));
                    }
                }
            }


            #region about MaxValue and MinValue

            for (int i = 0; i < propertyInfoOfAssignedProperties.Count; i++)
            {
                var optionAttribute = optionAttributesOfAssignedProperties[i];
                var propertyInfo = propertyInfoOfAssignedProperties[i];
                var maxValue = optionAttribute.MaxValue;
                var minValue = optionAttribute.MinValue;
                if ((maxValue == null) && (minValue == null))
                    continue;

                if (propertyInfo.PropertyType.IsArray)
                {
                    var array = propertyInfo.GetValue(optionsAssigned, null);
                    var list = array as Array;
                    if (list == null)
                        continue;

                    var firstMember = list.GetValue(0);
                    var memberType = firstMember.GetType();
                    if (!IsNumericType(memberType))
                    {
                        builder.AppendLine(String.Format("Options {0} is not of numeric type, and must not have either MaxValue or MinValue defined in CommandLineOptionAttribute.", propertyInfo.Name));
                        continue;
                    }

                    if (maxValue != null)
                    {
                        if (memberType.Equals(decimalType) && !decimalType.Equals(maxValue))
                        {
                            maxValue = Convert.ToDecimal(maxValue);
                        }

                        foreach (var m in list)
                        {
                            IComparable comparable = m as IComparable;
                            
                            if (comparable.CompareTo(maxValue) > 0)
                            {
                                builder.AppendLine(String.Format("In option {0}, this member {1} is greater than maxValue {2}.", propertyInfo.Name, m, maxValue));
                            }
                        }
                    }

                    if (minValue != null)
                    {
                        if (memberType.Equals(decimalType) && !decimalType.Equals(minValue))
                        {
                            minValue = Convert.ToDecimal(minValue);
                        }

                        foreach (var m in list)
                        {
                            IComparable comparable = m as IComparable;
                            if (comparable.CompareTo(minValue) < 0)
                            {
                                builder.AppendLine(String.Format("In option {0}, this member {1} is less than than minValue {2}.", propertyInfo.Name, m, minValue));
                            }
                        }
                    }
                }
                else
                {
                    if (!IsNumericType(propertyInfo.PropertyType))
                    {
                        builder.AppendLine(String.Format("Options {0} is not of numeric type, and must not have either MaxValue or MinValue defined in CommandLineOptionAttribute.", propertyInfo.Name));
                        continue;
                    }
                    if (maxValue != null)
                    {
                        var m = propertyInfo.GetValue(optionsAssigned, null);
                        IComparable comparable = m as IComparable;
                        if (propertyInfo.PropertyType.Equals(decimalType) && !decimalType.Equals(maxValue))
                        {
                            maxValue = Convert.ToDecimal(maxValue);
                        }

                        if (comparable.CompareTo(maxValue) > 0)
                        {
                            builder.AppendLine(String.Format("In option {0}, this member {1} is greater than maxValue {2}.", propertyInfo.Name, m, maxValue));
                        }
                    }

                    if (minValue != null)
                    {
                        var m = propertyInfo.GetValue(optionsAssigned, null);
                        IComparable comparable = m as IComparable;
                        if (propertyInfo.PropertyType.Equals(decimalType) && !decimalType.Equals(minValue))
                        {
                            minValue = Convert.ToDecimal(minValue);
                        }

                        if (comparable.CompareTo(minValue) < 0)
                        {
                            builder.AppendLine(String.Format("In option {0}, this member {1} is less than than minValue {2}.", propertyInfo.Name, m, minValue));
                        }
                    }
                }


            }
            #endregion

            return builder.ToString();
        }

        readonly static Type decimalType = typeof(decimal);

        internal static bool IsNumericType(Type type)
        {
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

    }
}
