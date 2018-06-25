using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Fonlow.CommandLineGui.Gui
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            lbVersion.Text = "Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lbCopyright.Text = "Copyright  ©  2009-2014  Zijian Huang";
            textBox1.Text = "License:  Mozilla Public License 1.1\r\n\r\n"+
                "Parts of Plossum 0.4, zlib/libpng License, Copyright © 2007  Peter Palotas\r\n" +
                "ANTLR 4, BSD License, Copyright © 2012 Terence Parr and Sam Harwell"
                ;
        }
    }
}
