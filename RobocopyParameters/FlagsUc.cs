using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Fonlow.CommandLineGui.Gui
{
    /// <summary>
    /// Provide checkedListBox to define flags. Each flag should be represented by one character.
    /// </summary>
    /// <typeparam name="U">U should be integer or enum, though the Where generic type constraint can not be int or enum.</typeparam>
    public partial class FlagsUC<TEnumType> : UserControl
    {
        public FlagsUC()
        {
            InitializeComponent();
        }

        public FlagsUC(TEnumType flags)
            : this()
        {
            checkedListBox.ColumnWidth = columnWidth;
            checkedListBox.Items.Clear();

            string[] items = GetFlagCharactersInLines().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            checkedListBox.Width = columnWidth * items.Length + 4;
            ClientSize = new Size(checkedListBox.Width + 6, ClientSize.Height);
            checkedListBox.Items.AddRange(items);

            FlaggedEnumConverter<TEnumType> converter = new FlaggedEnumConverter<TEnumType>();// explicit conversion is needed, as U can not be type casted into int.
            Flags = (int)converter.ConvertTo(flags, typeof(int));
        }

        const int columnWidth = 40;

        public int Flags
        {
            get
            {
                return GetFlags(checkedListBox);
            }

            set
            {
                LoadFlagsToCheckedList(value, checkedListBox);
            }
        }

        private static void LoadFlagsToCheckedList(int flags, CheckedListBox checkedListBox)
        {
            int flagTotal = checkedListBox.Items.Count;
            for (int i = 0; i < flagTotal; i++)
            {
                int flagMark = 1 << i;
                checkedListBox.SetItemChecked(i, (flagMark & flags) != 0);
            }
        }

        /// <summary>
        /// Get marked flags from a checkedListBox.
        /// </summary>
        /// <param name="checkedListBox"></param>
        /// <returns></returns>
        private static int GetFlags(CheckedListBox checkedListBox)
        {
            int flagTotal = checkedListBox.Items.Count;
            int flags = 0;
            for (int i = 0; i < flagTotal; i++)
            {
                int flagMark = 1 << i;
                if (checkedListBox.GetItemChecked(i))
                {
                    flags |= flagMark;
                }
            }
            return flags;
        }

        /// <summary>
        /// Analyze enum type U and build a string each of which character represents a flag.
        /// The characters are separated by line breaks.
        /// </summary>
        /// <returns></returns>
        static string GetFlagCharactersInLines()
        {
            string[] flagNames = Enum.GetNames(typeof(TEnumType));
            StringBuilder builder = new StringBuilder();

            foreach (string s in flagNames)
            {
                if (s.Length == 1)
                {
                    builder.Append(s + Environment.NewLine);
                }
            }
            return builder.ToString();
        }
    }
}
