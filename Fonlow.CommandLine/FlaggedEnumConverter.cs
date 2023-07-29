using System;
using System.Text;
using System.ComponentModel;

namespace Fonlow.CommandLineGui
{
    /// <summary>
    /// Explicityly convert a flagged enum into string or from int to string. Each flag of the enum can be represented by one character.
    /// </summary>
    /// <typeparam name="T">T must be enum.</typeparam>
    public class FlaggedEnumConverter<T> : TypeConverter
    {
        public FlaggedEnumConverter()
        {
            flagNames = Enum.GetNames(typeof(T));
            StringBuilder builder = new StringBuilder();
            foreach (string s in flagNames)
            {
                if (s.Length == 1)
                {
                    builder.Append(s);
                }
            }
            flagCharacters = builder.ToString();
            flagTotal = flagNames.Length - 1;
        }

        int flagTotal;
        string[] flagNames;
        string flagCharacters;
        const string noneString = "None";

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            // no need to convert from int explicitly.

            return base.CanConvertFrom(context, sourceType);
        }


        /// <summary>
        /// convert to string representation, or to int type.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1820:TestForEmptyStringsUsingStringLength")]
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                int flagsInt = (int)value;
                string str = String.Empty;
                for (int i = 0; i < flagTotal; i++)
                {
                    int flagMark = 1 << i;
                    if ((flagMark & flagsInt) != 0)
                    {
                        str += flagCharacters[i];
                    }
                }

                if (str == string.Empty)
                    return noneString;
                else
                    return str;

            }
            else if (destinationType == typeof(int))// this is used for generic while implicity constraint of enum is applied.
            {
                return (int)value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string str = value as string;
            if (str != null)
            {
                char[] v = str.ToCharArray();
                int flagsInt = 0;
                foreach (char s in v)
                {
                    int index = flagCharacters.IndexOf(Char.ToUpper(s));
                    if (index >= 0)
                    {
                        flagsInt |= 1 << index;
                    }
                    else // so invalid character in the string
                    {
                        return null;
                    }
                }
                return flagsInt;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }


}
