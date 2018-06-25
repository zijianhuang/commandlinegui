using System;
using System.Reflection;
using System.ComponentModel;

namespace Fonlow.CommandLine
{
    internal static class PropertyHelper
    {
        internal static string GetPropertyDescription(PropertyInfo propertyInfo)
        {
            var a = ReadAttribute<DescriptionAttribute>(propertyInfo);
            return a == null ? null : a.Description;
        }

        internal static string SimplyQuoteString(string s)
        {
            if (s.Contains(" "))
            {
                return "\"" + s + "\"";
            }

            return s;
        }

        internal static string GetDisplayName(PropertyInfo propertyInfo)
        {
            var a = ReadAttribute<DisplayNameAttribute>(propertyInfo);
            return a == null ? propertyInfo.Name : a.DisplayName;
        }

        /// <summary>
        /// Return the order of the parameter. If not defined, return int.Max
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        internal static int GetParameterOrder(PropertyInfo propertyInfo)
        {
            var a = ReadAttribute<ParameterOrderAttribute>(propertyInfo);
            return a == null ? int.MaxValue : a.Order;
        }

        internal static T ReadAttribute<T>(MemberInfo memberInfo) where T : Attribute
        {
            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            object[] objects = memberInfo.GetCustomAttributes(typeof(T), false);
            if (objects.Length == 1)
            {
                return (objects[0] as T);
            }
            return null;
        }

        internal static T ReadAttribute<T>(Type type) where T : Attribute
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            object[] objects = type.GetCustomAttributes(typeof(T), false);
            if (objects.Length == 1)
            {
                return (objects[0] as T);
            }
            return null;
        }

        /// <summary>
        /// Read default value according to DefaultValueAttribute. If the attribute is not available, use the default value of the type defined by this library.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        internal static object ReadDefaultValue(PropertyInfo propertyInfo)
        {
            object[] objects = propertyInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);
            if (objects.Length == 1)
            {
                return (objects[0] as DefaultValueAttribute).Value;
            }

            return GetDefaultValueOfType(propertyInfo.PropertyType);
        }

        internal static object GetDefaultValueOfType(Type PropertyType)
        {
            if (PropertyType == typeof(string))
            {
                return String.Empty;
            }

            if (PropertyType == typeof(bool))
            {
                return false;
            }

            if (PropertyType == typeof(int))
            {
                return 0;
            }

            if (PropertyType == typeof(double))
            {
                return 0.0d;
            }

            if (PropertyType == typeof(float))
            {
                return 0.0f;
            }

            if (PropertyType == typeof(decimal))
            {
                return 0m;
            }

            if (PropertyType == typeof(uint))
            {
                return 0u;
            }

            if (PropertyType == typeof(long))
            {
                return 0L;
            }

            if (PropertyType == typeof(ulong))
            {
                return 0UL;
            }

            if (PropertyType.IsEnum)
            {
                return 0;
            }

            return null;
        }

        /// <summary>
        /// Create a converter according to TypeConverterAttribute of t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Null if no TypeConverterAttribute.</returns>
        internal static TypeConverter CreateTypeConverter(Type t)
        {
            AttributeCollection attributeCollection = TypeDescriptor.GetAttributes(t);
            TypeConverterAttribute converterAttribute = attributeCollection[typeof(TypeConverterAttribute)] as TypeConverterAttribute;

            if ((converterAttribute != null) && (!String.IsNullOrEmpty(converterAttribute.ConverterTypeName)))
            {
                Type converterType = Type.GetType(converterAttribute.ConverterTypeName);
                if (converterType != null)
                {
                    TypeConverter converter = Activator.CreateInstance(converterType) as TypeConverter;
                    return converter;
                }
            }

            return null;
        }


    }

}
