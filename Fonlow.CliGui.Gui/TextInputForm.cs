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
    public partial class TextInputForm : Form
    {
        public TextInputForm()
        {
            InitializeComponent();
        }

        public static string Input(IWin32Window handle, string title, string caption)
        {
            using (var form = new TextInputForm())
            {
                form.Text = title;
                form.label1.Text = caption;
                if (form.ShowDialog(handle) == DialogResult.OK)
                {
                    return form.textBox1.Text;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
