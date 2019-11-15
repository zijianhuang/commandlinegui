using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Diagnostics;

namespace Fonlow.CommandLineGui
{
    public sealed class Settings : ApplicationSettingsBase
    {

        private static Settings defaultInstance = ((Settings)(ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get
            {
                return defaultInstance;
            }
        }

        [UserScopedSetting]
        [DefaultSettingValue("RobocopyParameters")]
        public string AssemblyNameOfCommand
        {
            get
            {
                return ((string)(this["AssemblyNameOfCommand"]));
            }

            set
            {
                this["AssemblyNameOfCommand"] = value;
            }

        }


        [ApplicationScopedSetting]
        public string[] AssemblyNames
        {
            get
            {
                return ((string[])(this["AssemblyNames"]));
            }
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("Mono")]
        public PluginAllocationMethod PluginAllocationMethod
        {
            get
            {
                return ((PluginAllocationMethod)(this["PluginAllocationMethod"]));
            }

        }

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
