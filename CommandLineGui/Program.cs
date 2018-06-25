using System;
using System.Windows.Forms;
using Fonlow.CommandLineGui.Gui;
using System.Linq;

namespace Fonlow.CommandLineGui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            switch (Settings.Default.PluginAllocationMethod)
            {
                case PluginAllocationMethod.Mono:
                    string assemblyName = Settings.Default.AssemblyNames.FirstOrDefault();//Better to use the first in the list rather than AssemblyNameOfCommand which is in UserScope.
                    if (String.IsNullOrEmpty(assemblyName))
                        assemblyName = "RobocopyParameters";
                    var command = CommandFactory.CreateCommandFromAssembly(assemblyName);
                    if (command != null)
                    {
                        using (MainForm form = new MainForm(command))
                        {
                            form.Left = 0;
                            form.Top = 0;
                            form.Height = Screen.PrimaryScreen.WorkingArea.Height;

                            Application.Run(form);
                        }
                    }
                    else
                    {
                        System.Diagnostics.Trace.TraceError("fail");
                        MessageBox.Show(String.Format("Can not load {0} to initialize.", assemblyName));
                    }

                    break;
                case PluginAllocationMethod.Registration:
                    RunMainFormWithMultipleCommands();
                    break;
                default:
                    throw new InvalidOperationException("Is this possible?");
            }
        }

        static void RunMainFormWithMultipleCommands()
        {
            using (MainForm form = new MainForm(Settings.Default.AssemblyNames, Settings.Default.AssemblyNameOfCommand))
            {
                form.Left = 0;
                form.Top = 0;
                form.Height = Screen.PrimaryScreen.WorkingArea.Height;

                Application.Run(form);
            }

        }
    }
}
