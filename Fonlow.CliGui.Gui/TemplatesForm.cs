using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fonlow.CommandLineGui.Gui
{
    public partial class TemplatesForm : Form
    {
        public TemplatesForm()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetAllChecked(checkBox1.Checked);
        }

        void SetAllChecked(bool y)
        {
            
            try
            {
                checkedListBox1.BeginUpdate();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, y);
                }
            }
            finally
            {
                checkedListBox1.EndUpdate();
            }
        }

        public IEnumerable<string> CheckedItems 
        {
            get
            {
                return checkedListBox1.CheckedItems.OfType<string>();
            }
        }

        public void SetItems(IEnumerable<string> items)
        {
            if (items == null)
                return;

            checkedListBox1.Items.Clear();
            try
            {
                checkedListBox1.BeginUpdate();
                checkedListBox1.Items.AddRange(items.ToArray());
            }
            finally
            {
                checkedListBox1.EndUpdate();
            }
        }
    }
}
