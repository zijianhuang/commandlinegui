using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace Fonlow.CommandLineGui.Gui
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        public EditorForm(PropertyGrid propertyGrid)
        {
            InitializeComponent();
            this.propertyGrid = propertyGrid;

            textBox.Multiline = true;
            textBox.WordWrap = true;
            textBox.Font = new Font("Courier New", 10, FontStyle.Regular);
            textBox.DetectUrls = false;
            textBox.SelectionChanged += new EventHandler(textBox_SelectionChanged);

            Text = "Command";
            ShowInTaskbar = true;
        }

        ICommand commandLoaded;

        PropertyGrid propertyGrid;

        public TextBoxBase TextBox
        {
            get
            {
                return textBox;
            }
        }

        public void SetCommandLoaded(ICommand command)
        {
            commandLoaded = command;
            RefreshTemplatesDropdown();
        }

        void RefreshTemplatesDropdown()
        {
            templatesDropdown.DropDownItems.Clear();

            var templateNames = CommandsTemplates.Instance.GetTemplatesNames(commandLoaded.CommandName);
            if (templateNames == null)
            {
                templatesDropdown.Text = "Templates(0)";
                return;
            }

            foreach (var item in templateNames)
            {
                templatesDropdown.DropDownItems.Add(item, null, templateNameClick);
            }

            templatesDropdown.Text = String.Format("Templates({0})", templateNames.Count());
        }

        void templateNameClick(object sender, EventArgs e)
        {
            string templateName = (sender as ToolStripDropDownItem).Text;
            try
            {
                string command = CommandsTemplates.Instance.Templates[commandLoaded.CommandName][templateName];
                textBox.Text = command;
                ParseCommandLineToGrid();
            }
            catch (System.Collections.Generic.KeyNotFoundException ex )
            {
                AppTraceSource.Instance.TraceWarning(ex.Message);
                return;
            }

            ParseCommandLineToGrid();
        }

        /// <summary>
        /// Telling the world what parameter selected through the WordSelected event being fired.
        /// </summary>
        void textBox_SelectionChanged(object sender, EventArgs e)
        {
            int selectionStart = textBox.SelectionStart;
            if (selectionStart == textBox.Text.Length)
                selectionStart--;

            string parameterText = null;

            if (CursorPositionChanged != null)
            {
                ParameterHighlightedEventArgs args = new ParameterHighlightedEventArgs(textBox.Text, selectionStart);
                CursorPositionChanged(this, args);//so the event handler will assign args.Parameter.
                parameterText = args.Parameter;
                if (String.IsNullOrEmpty(parameterText))
                    return;

                if (WordSelected != null)
                {
                    WordSelected(this, new TextMessageEventArgs(parameterText));
                }
            }


        }

        /// <summary>
        /// Fired when mouse clicking on a parameter in the commandline.
        /// </summary>
        public event EventHandler<TextMessageEventArgs> WordSelected;

        /// <summary>
        /// Fired when mouse clicking anywhere in the commandline. This event
        /// is fired before WordSelected, and may or may not fire WordSelected event.
        /// </summary>
        public event EventHandler<ParameterHighlightedEventArgs> CursorPositionChanged;



        private void btnCopy_Click(object sender, EventArgs e)
        {
            ValidateAndCopy();
        }

        private void ValidateAndCopy()
        {
            ValidateCommandLine();
            Clipboard.SetText(textBox.Text);
        }

        private void ValidateCommandLine()
        {
            ParseCommandLineToGrid();
            ReflectFromGridToCommandLine();
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            textBox.Text = Clipboard.GetText(TextDataFormat.Text);
        }

        private void ReflectFromGridToCommandLine()
        {
            textBox.Clear();
            textBox.Text = commandLoaded.CommandLine;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void ParseCommandLineToGrid()
        {
            if (String.IsNullOrWhiteSpace(textBox.Text))
                return;

            if (commandLoaded.ParseCommandLine(textBox.Text))
            {
                propertyGrid.Refresh();
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            RunCommand();
        }

        Process process;

        OutputWindow outputWin;

        void RunCommand()
        {
            ParseCommandLineToGrid();
            process = new Process();
            process.StartInfo.FileName = commandLoaded.CommandName;
            process.StartInfo.Arguments = commandLoaded.DefinedParametersAndOptions;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            if (outputWin == null)
            {
                outputWin = new OutputWindow();
                outputWin.Window.Left = Left;
                outputWin.Window.Top = Top + Height;
                outputWin.Window.Width = Width;
                outputWin.Window.Height = 300;
                outputWin.Window.StartPosition = FormStartPosition.Manual;
                outputWin.Window.FormClosing += new FormClosingEventHandler(OutputWindow_FormClosing);
                outputWin.Window.Show();
            }
            else
            {
                outputWin.TextBox.Clear();
            }

            process.Start();
            process.BeginOutputReadLine();
        }

        void RunCommandInCommandPrompt()
        {
            ParseCommandLineToGrid();
            process = new Process();
            process.StartInfo.FileName = commandLoaded.CommandName;
            process.StartInfo.Arguments = commandLoaded.DefinedParametersAndOptions;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardOutput = false;
  //          process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            if (outputWin == null)
            {
                outputWin = new OutputWindow();
                outputWin.Window.Left = Left;
                outputWin.Window.Top = Top + Height;
                outputWin.Window.Width = Width;
                outputWin.Window.Height = 300;
                outputWin.Window.StartPosition = FormStartPosition.Manual;
                outputWin.Window.FormClosing += new FormClosingEventHandler(OutputWindow_FormClosing);
                outputWin.Window.Show();
            }
            else
            {
                outputWin.TextBox.Clear();
            }

            process.Start();
        }

        void OutputWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            outputWin = null;
        }


        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            AddOutputLine(e.Data);
        }

        void AddOutputLine(string s)
        {
            if (s == null)
                s = String.Empty;

            if (outputWin.Window.InvokeRequired)
            {
                Action<string> d = new Action<string>(AddOutputLine);
                Invoke(d, new object[] { s });
            }
            else
            {
                outputWin.TextBox.AppendText(s);
                outputWin.TextBox.AppendText(Environment.NewLine);
            }
        }

        private void parseButton_Click(object sender, EventArgs e)
        {
            ParseCommandLineToGrid();
        }

        private void runCmdButton_Click(object sender, EventArgs e)
        {
            RunCommandInCommandPrompt();
        }

        void SaveCommandLine()
        {
            Here:
            string templateName = TextInputForm.Input(this, String.Format("Save for {0}", commandLoaded.CommandName), "Template description");

            if (!CommandsTemplates.Instance.AddTemplate(commandLoaded.CommandName, templateName, textBox.Text, false))
            {
                var r = MessageBox.Show(this, String.Format("Do you want to overwrite existing template ({0})", templateName), "Confirmation", MessageBoxButtons.YesNoCancel);
                if (r == System.Windows.Forms.DialogResult.Yes)
                {
                    CommandsTemplates.Instance.AddTemplate(commandLoaded.CommandName, templateName, textBox.Text, true);
                }
                else if (r== System.Windows.Forms.DialogResult.No)
                {
                    goto Here;
                }
            }

            RefreshTemplatesDropdown();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCommandLine();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteTemplates();
        } 
      
        void DeleteTemplates()
        {
            using (var form = new TemplatesForm())
            {
                form.Text = String.Format("Templates of {0}", commandLoaded.CommandName);
                var templatesNames = CommandsTemplates.Instance.GetTemplatesNames(commandLoaded.CommandName);
                if (templatesNames == null)
                {
                    MessageBox.Show(this, String.Format("Nothing to delete for command {0}.", commandLoaded.CommandName));
                    return;
                }

                form.SetItems(templatesNames);
                if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var checkedItem = form.CheckedItems;
                    foreach (var item in checkedItem)
                    {
                        CommandsTemplates.Instance.DeleteTemplate(commandLoaded.CommandName, item);
                    }

                    RefreshTemplatesDropdown();
                }
            }
        }
    }
}
