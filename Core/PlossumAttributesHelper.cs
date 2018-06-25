using System;
using System.Collections.Generic;
using System.Linq;
using Plossum.CommandLine;
using System.Reflection;

namespace Fonlow.CommandLine
{
    public static class PlossumAttributesHelper
    {
        public static CommandLineOptionAttribute GetCommandLineOptionAttribute(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            object[] objects = memberInfo.GetCustomAttributes(typeof(CommandLineOptionAttribute), false);
            if (objects.Length == 1)
            {
                return (objects[0] as CommandLineOptionAttribute);
            }
            return null;
        }


        public static CommandLineManagerAttribute GetCommandLineManagerAttribute(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var customAttributes = type.GetCustomAttributes(typeof(CommandLineManagerAttribute), false);
            if (customAttributes.Length == 1)
            {
                return (customAttributes[0] as CommandLineManagerAttribute);
            }
            return null;
        }

        public static Dictionary<string, CommandLineOptionGroupAttribute> GetCommandLineOptionGroupAttributesDic(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var dic = new Dictionary<string, CommandLineOptionGroupAttribute>();
            var customAttributes = type.GetCustomAttributes(typeof(CommandLineOptionGroupAttribute), false);
            foreach (var item in customAttributes)
            {
                var att = item as CommandLineOptionGroupAttribute;
                dic.Add(att.Id, att);
            }
            return dic;
        }

        public static CommandLineOptionAttribute[] GetCommandLineOptionAttributes(Type optionsType)
        {
            if (optionsType == null)
                throw new ArgumentNullException("optionsType");

            var propertiesOfOptions = optionsType.GetProperties();
            var r = propertiesOfOptions
                .Select((propertyItem) => ReadCommandLineOptionAttribute(propertyItem))
                .Where(d => d != null)
                .ToArray();

            var c = propertiesOfOptions.Count() - r.Length;
            if (c > 0)
            {
                System.Diagnostics.Trace.TraceError("You programmer have some properties in optionsType not decorated by CommandLineOptionAttribute. The total number of offenders is {0}.", c);
            }

            return r;
        }

        public static PropertyInfo[] GetOptionProperties(Type optionsType)
        {
            if (optionsType == null)
                throw new ArgumentNullException("optionsType");

            var propertiesOfOptions = optionsType.GetProperties();
            var r = propertiesOfOptions
                .Where((propertyItem) => { var a = ReadCommandLineOptionAttribute(propertyItem); return a != null; })
                .ToArray();

            return r;
        }

        internal static CommandLineOptionAttribute ReadCommandLineOptionAttribute(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            object[] objects = memberInfo.GetCustomAttributes(typeof(CommandLineOptionAttribute), false);
            if (objects.Length == 1)
            {
                var optionAttribute = objects[0] as CommandLineOptionAttribute;
                if (optionAttribute != null)
                {
                    if (String.IsNullOrWhiteSpace(optionAttribute.Name))
                    {
                        optionAttribute.Name = memberInfo.Name;
                    }
                }

                return optionAttribute;
            }
            return null;
        }



    }




}
