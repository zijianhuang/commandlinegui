namespace Fonlow.CommandLineGui
{
	public sealed class Settings
	{
		public string AssemblyNameOfCommand { get; set; } = "RobocopyParameters";

		public string[] AssemblyNames { get; set; }

		public PluginAllocationMethod PluginAllocationMethod { get; set; } = PluginAllocationMethod.Mono;

	}

	/// <summary>
	/// Mono: locate only one assembly in property AssemblyNameOfCommand,
	/// Registration: plugin files in the same directory of commandLineGui.exe.
	/// </summary>
	public enum PluginAllocationMethod
	{
		Mono, 
		Registration
	};


}
