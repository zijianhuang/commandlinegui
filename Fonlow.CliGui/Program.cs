using System;
using System.Windows.Forms;
using Fonlow.CommandLineGui.Gui;
using System.Linq;
using Microsoft.Extensions.Configuration;

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

			System.Reflection.Assembly appAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			string dirOfAppAssembly = System.IO.Path.GetDirectoryName(appAssembly.Location);
			IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile(System.IO.Path.Combine(dirOfAppAssembly, "appsettings.json")).Build();
			var appSettings = config.GetSection("cliSettings");
			var cliSettings = appSettings.Get<Settings>();
			
			switch (cliSettings.PluginAllocationMethod)
			{
				case PluginAllocationMethod.Mono:
					string assemblyName = cliSettings.AssemblyNames.FirstOrDefault();//Better to use the first in the list rather than AssemblyNameOfCommand which is in UserScope.
					if (String.IsNullOrEmpty(assemblyName))
						assemblyName = "RobocopyParameters";
					var command = CommandFactory.CreateCommandFromAssembly(assemblyName);
					if (command != null)
					{
						using (MainForm form = new MainForm(command, cliSettings))
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
					RunMainFormWithMultipleCommands(cliSettings);
					break;
				default:
					throw new InvalidOperationException("Is this possible?");
			}
		}

		static void RunMainFormWithMultipleCommands(Settings settings)
		{
			using (MainForm form = new MainForm(settings.AssemblyNames, settings.AssemblyNameOfCommand))
			{
				form.Left = 0;
				form.Top = 0;
				form.Height = Screen.PrimaryScreen.WorkingArea.Height;

				Application.Run(form);
			}

		}
	}
}
