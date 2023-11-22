using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Permissions;
using System.Linq;

namespace Fonlow.CommandLineGui.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            commandsBuffer = new Dictionary<string, ICommand>();
        }

        ICommand commandLoaded;

        string defaultAssemblyName;

        Dictionary<string, ICommand> commandsBuffer;

        readonly Settings settings;


		public MainForm(ICommand command, Settings settings)
            : this()
        {
            this.settings = settings;
            CreateEditorWindow();
            if (command == null)
                throw new ArgumentNullException("command");
            commandsDropDown.Visible = false;
            LoadCommand(command);
            InitEventHandlers();
        }

        public MainForm(IEnumerable<string> assemblyNames, string defaultAssemblyName)
            : this()
        {
            CreateEditorWindow();
            this.defaultAssemblyName = defaultAssemblyName;

            try
            {
                InitCommandDropDownAndLoadDefautCommand(assemblyNames);
            }
            catch (InvalidOperationException e)
            {
                AppTraceSource.Instance.TraceWarning(e.Message);
                MessageBox.Show(this, e.Message, "Error", MessageBoxButtons.OK);
                toolStrip1.Enabled = false;
                return;//the main form still shows, but user should close.

            } 
            
            InitEventHandlers();

        }

        void CreateEditorWindow()
        {
            editorWindow = new EditorForm(propertyGrid1);
            textBox = editorWindow.TextBox;
            //    textBox.TextChanged += new EventHandler(textBox_TextChanged);
            editorWindow.Left = Left + Width;
            editorWindow.Top = Top;
            editorWindow.Height = 100;
            editorWindow.Width = Screen.PrimaryScreen.WorkingArea.Width - Width;
            editorWindow.StartPosition = FormStartPosition.Manual;
            editorWindow.CursorPositionChanged += new EventHandler<ParameterHighlightedEventArgs>(editorWindow_CursorPositionChanged);
            editorWindow.WordSelected += new EventHandler<TextMessageEventArgs>(editorWindow_WordSelected);
            editorWindow.FormClosing += new FormClosingEventHandler(Window_FormClosing);
        }

        void InitCommandDropDownAndLoadDefautCommand(IEnumerable<string> assemblyNames)
        {
            commandsDropDown.Visible = true;
            ICommand defaultCommand=null;
            foreach (var text in assemblyNames)
            {
                var command = CommandFactory.CreateCommandFromAssembly(text);
                if (command == null)
                    continue;

                var menuItem = commandsDropDown.DropDownItems.Add(command.CommandName, null, ToolStripDropDownItemHandler);
                menuItem.ToolTipText = String.Format("{0} in assembly {1}", command.CommandName, text);
                commandsBuffer.Add(command.CommandName, command);

                if (text.Equals(defaultAssemblyName, StringComparison.CurrentCultureIgnoreCase))
                {
                    defaultCommand = command;
                }
            }

            if (commandsBuffer.Count == 0)
            {
                throw new InvalidOperationException("The program cannot find any assembly containing ICommand, and the program will quit.");
            }

            if (defaultCommand != null)
            {
                LoadCommand(defaultCommand);
            }
            else
            {
                LoadCommand(commandsBuffer.First().Value);
                defaultAssemblyName = assemblyNames.First();
            }
        }

        void ToolStripDropDownItemHandler(object sender, EventArgs args)
        {
            string commandName = (sender as ToolStripDropDownItem).Text;
            var command = commandsBuffer[commandName];
            if (command == null)
            {
                MessageBox.Show(this, String.Format("Loading {0} has some problem.", commandName), "Warning");
                return;
            }
            LoadCommand(command);
            defaultAssemblyName = command.GetType().Assembly.GetName().Name;
        }

        void InitEventHandlers()
        {
            propertyGrid1.PropertyValueChanged += propertyGrid1_PropertyValueChanged;
            propertyGrid1.SelectedGridItemChanged += new SelectedGridItemChangedEventHandler(propertyGrid1_SelectedGridItemChanged);

            Shown += new EventHandler(MainForm_Shown);
            FormClosing += MainForm_FormClosing;
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            settings.AssemblyNameOfCommand = defaultAssemblyName;
            //Settings.Default.Save(); //todo: do I need to save
            CommandsTemplatesFile.Save(CommandsTemplates.Instance);
        }

        void LoadCommand(ICommand command)
        {
            Text = command.CommandName;
            this.options = command.ParametersAndOptionsProxy;
            this.commandLoaded = command;
            propertyGrid1.SelectedObject = options;
            editorWindow.SetCommandLoaded(command);

       //     if (command.ReportErrorHandler !=null)
            {
                command.ReportErrorHandler += command_ReportErrorHandler;
            }   
        }

        void command_ReportErrorHandler(object sender, TextMessageEventArgs e)
        {
            MessageBox.Show(this, e.Message, "Error");
        }

        void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            if (editorEditing)
            {
                editorEditing = false;
                return;
            }

            if (e.NewSelection.PropertyDescriptor != null)
            {
                textBoxHint.Text = e.NewSelection.PropertyDescriptor.Description;
            }

            AppTraceSource.Instance.TraceEvent(TraceEventType.Verbose, 0, "selected item changed: " + e.NewSelection.Label);
        }

        TextBoxBase textBox;

        bool editorEditing;

        EditorForm editorWindow;

        void MainForm_Shown(object sender, EventArgs e)
        {
            editorWindow.Show();
        }

        void editorWindow_CursorPositionChanged(object sender, ParameterHighlightedEventArgs e)
        {
            e.Parameter = commandLoaded.PickupParameterAtPosition(e.CommandText, e.CursorPosition);
        }

        void Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Close();
        }

        void editorWindow_WordSelected(object sender, TextMessageEventArgs e)
        {
            editorEditing = true;
            textBoxHint.Text = e.Message + ": " + commandLoaded.GetParameterDescription(e.Message);
        }

        object options;

        public object CommandOptions { get { return options; } }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            ReflectFromGridToCommandLine();
        }

        private void ReflectFromGridToCommandLine()
        {
            textBox.Clear();
            textBox.Text = commandLoaded.CommandLine;
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            ShowAbout();
        }

        private void ShowAbout()
        {
            using (var form = new AboutForm())
            {
                form.ShowDialog(this);
            }
        }


        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoHome();
        }

        static void GoHome()
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/commandlinegui");
        }


    }


}
