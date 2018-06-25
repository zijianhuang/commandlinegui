using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plossum.CommandLine;

namespace TestBetter
{
    [CommandLineManager(ApplicationName = "MyPlossum", Description = "Demostrate the power of Plossum",  Copyright = "Fonlow (c) 2013", Version = "1.1")]
    [CommandLineOptionGroup("detail", Name = "Detail")]
    [CommandLineOptionGroup("other", Name = "Other")]
    public class Options
    {
        public Options()
        {
        }
        [CommandLineOption(Aliases = "F", Description = "Function name, e.g., /F=FirstFunction")]
        public string Function { get; set; }


        [CommandLineOption(Aliases = "du", Name = "Duration", Description = "Duration in second.", RequireExplicitAssignment = true, DefaultAssignmentValue = 3600.08d, GroupId = "detail")]
        public double DoubleP { get; set; }

        [CommandLineOption]
        public int IntP { get; set; }

        [CommandLineOption]
        public short ShortP { get; set; }

        [CommandLineOption]
        public byte ByteP { get; set; }

        [CommandLineOption(Aliases = "FI, FIL", Name = "Filters", Description = "Filters", RequireExplicitAssignment = false, GroupId = "detail")]
        public string[] Filters { get; set; }

        [CommandLineOption(Aliases = "h", Description = "Shows this help text", GroupId = "other")]
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

        [CommandLineOption]
        public sbyte SbyteP { get; set; }

        [CommandLineOption]
        public char CharP { get; set; }

        [CommandLineOption]
        public decimal DecimalP { get; set; }
        [CommandLineOption]
        public float FloatP { get; set; }
        [CommandLineOption]
        public uint UintP { get; set; }
        [CommandLineOption]
        public long LongP { get; set; }
        [CommandLineOption]
        public ulong UlongP { get; set; }

        [CommandLineOption]
        public ushort UShortP { get; set; }


        [CommandLineOption]
        public decimal[] DecimalsP { get; set; }
        [CommandLineOption]
        public float[] FloatsP { get; set; }
        [CommandLineOption]
        public uint[] UintsP { get; set; }


        /*
bool
byte
sbyte
char
decimal
double
float
int
uint
long
ulong
short
ushort
string
enum
*/

    }

    [Flags]
    public enum MyEnum { None = 0, Hello = 1, World = 2, Command = 4 };

    [CommandLineManager(ApplicationName = "MyPicky")]
    [CommandLineOptionGroup("atMost1", Name = "AtMostOne", Require = OptionGroupRequirement.AtMostOne)]
    [CommandLineOptionGroup("atLeast1", Name = "AtLeastOne", Require = OptionGroupRequirement.AtLeastOne)]
    [CommandLineOptionGroup("exactly1", Name = "ExactlyOne", Require = OptionGroupRequirement.ExactlyOne)]
    [CommandLineOptionGroup("all", Name = "All", Require = OptionGroupRequirement.All)]
    public class PickyOptions
    {
        [CommandLineOption(GroupId = "atMost1")]
        public string Option1AtMostOne { get; set; }

        [CommandLineOption(GroupId = "atMost1")]
        public string Option2AtMostOne { get; set; }

        [CommandLineOption(GroupId = "atLeast1")]
        public string Option1AtLeastOne { get; set; }

        [CommandLineOption(GroupId = "atLeast1")]
        public string Option2AtLeastOne { get; set; }

        [CommandLineOption(GroupId = "exactly1")]
        public string Option1ExactlyOne { get; set; }

        [CommandLineOption(GroupId = "exactly1")]
        public string Option2ExactlyOne { get; set; }

        [CommandLineOption(GroupId = "all")]
        public string Option1All { get; set; }

        [CommandLineOption(GroupId = "all")]
        public string Option2All { get; set; }
    }

    [CommandLineManager(ApplicationName = "MyPicky")]
    [CommandLineOptionGroup("atMost1", Name = "AtMostOne")]
    [CommandLineOptionGroup("atLeast1", Name = "AtLeastOne")]
    [CommandLineOptionGroup("exactly1", Name = "ExactlyOne")]
    [CommandLineOptionGroup("all", Name = "All")]
    public class POptions
    {
        [CommandLineOption(GroupId = "atMost1", Prohibits = "Option2AtMostOne Option2AtLeastOne")]//by property names
        public string Option1AtMostOne { get; set; }

        [CommandLineOption(GroupId = "atMost1")]
        public string Option2AtMostOne { get; set; }

        [CommandLineOption(GroupId = "atLeast1")]
        public string Option1AtLeastOne { get; set; }

        [CommandLineOption(GroupId = "atLeast1", Prohibits = "Option1All,Op2eOne OptiontwoAll, O2E1")]//by property name, alias, and option name
        public string Option2AtLeastOne { get; set; }

        [CommandLineOption(GroupId = "exactly1")]
        public string Option1ExactlyOne { get; set; }

        [CommandLineOption(GroupId = "exactly1", Aliases = "O2E1,Op2EOne")]
        public string Option2ExactlyOne { get; set; }

        [CommandLineOption(GroupId = "all")]
        public string Option1All { get; set; }

        [CommandLineOption(GroupId = "all", Name = "OptionTwoAll")]
        public string Option2All { get; set; }
    }

    [CommandLineManager(ApplicationName = "MyPicky")]
    public class MaxOccursOptions
    {
        [CommandLineOption(MaxOccurs = 3, MinOccurs = 2)]
        public string[] Files { get; set; }


    }

    [CommandLineManager(ApplicationName = "MyPicky")]
    public class MaxOccursOptionsSilly
    {
        [CommandLineOption(MaxOccurs = 3)]
        public string Silly { get; set; }

        [CommandLineOption]
        public int Integer { get; set; }
    }

    [CommandLineManager(ApplicationName = "MyPicky")]
    public class NumericOptions
    {
        [CommandLineOption(MaxValue = 10000, MinValue = 10)]
        public int Money { get; set; }

        [CommandLineOption(MaxValue = 10000, MinValue = 10)]
        public int[] Prices { get; set; }

    }

    /// <summary>
    /// decimal can not be attribute arguments since decimal is structure rather than premitive type.
    /// </summary>
    [CommandLineManager(ApplicationName = "MyPicky")]
    public class DecimalOptions
    {
        [CommandLineOption(MaxValue = 10000, MinValue = 10)]
        public decimal Money { get; set; }

        [CommandLineOption(MaxValue = 10000, MinValue = 10)]
        public decimal[] Prices { get; set; }

    }


}
