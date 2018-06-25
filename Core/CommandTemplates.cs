using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Configuration;
using System.IO;

namespace Fonlow.CommandLineGui
{
    /// <summary>
    /// To store templates for each command
    /// </summary>
    [DataContract(Namespace = "http://wwww.fonlow.com/CommandLineGui/2013/08")]
    public class CommandsTemplates
    {
        private CommandsTemplates()
        {
            Templates =  CommandsTemplatesFile.Load();
            if (Templates == null)
            {
                Templates = new CommandsTemplatesDictionary();
            }
        }

        [DataMember]
        public CommandsTemplatesDictionary Templates { get; private set; }

        public bool Modified { get; private set; }

        public void MarkDirty()
        {
            Modified = true;
        }

        public static CommandsTemplates Instance { get { return Nested.instance; } }

        private static class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly CommandsTemplates instance = new CommandsTemplates();
        }


    }

    [Serializable]
    public sealed class CommandsTemplatesDictionary : Dictionary<string, Dictionary<string, string>>
    {
        public CommandsTemplatesDictionary()
        {

        }

        CommandsTemplatesDictionary(SerializationInfo info,
           StreamingContext context):base(info, context)

        {

        }
    }

    public static class CommandsTemplatesFile
    {
        static string GetDefaultExeConfigPath(ConfigurationUserLevel userLevel)
        {
            string s;
            try
            {
                var UserConfig = ConfigurationManager.OpenExeConfiguration(userLevel);
                s = UserConfig.FilePath;
            }
            catch (ConfigurationException e)
            {
                s = e.Filename;
            }

            var directoryName = Path.GetDirectoryName(s);
            Directory.CreateDirectory(directoryName);
            return Path.Combine(directoryName, "CommandTemplates.xml");
        }


        internal static CommandsTemplatesDictionary Load()
        {
            try
            {
                string filePath = GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoamingAndLocal);
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var serializer = new DataContractSerializer(typeof(CommandsTemplatesDictionary));
                    return serializer.ReadObject(fs) as CommandsTemplatesDictionary;
                }
            }
            catch (IOException e)
            {
                AppTraceSource.Instance.TraceWarning("When writing CommandTemplates: " + e.ToString());
                return new CommandsTemplatesDictionary();
            }
            catch (SerializationException e)
            {
                AppTraceSource.Instance.TraceWarning("When writing CommandTemplates: " + e.ToString());
                return new CommandsTemplatesDictionary();
            }
        }

        public static void Save(CommandsTemplates commandsTemplates)
        {
            if (!commandsTemplates.Modified)
                return;

            string filePath = GetDefaultExeConfigPath(ConfigurationUserLevel.PerUserRoamingAndLocal);
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                var serializer = new DataContractSerializer(typeof(CommandsTemplatesDictionary));
                serializer.WriteObject(fs, commandsTemplates.Templates);
            }
        }

    }

    public static class CommandsTemplatesExtension
    {
        public static void DeleteTemplate(this CommandsTemplates commandsTemplates, string commandName, string templateDescription)
        {
            Dictionary<string, string> commandDic;
            if (commandsTemplates.Templates.TryGetValue(commandName, out commandDic))
            {
                if (commandDic.Remove(templateDescription))
                {
                    commandsTemplates.MarkDirty();
                }
            }
        }

        public static IEnumerable<string> GetTemplatesNames(this CommandsTemplates commandsTemplates, string commandName)
        {
            Dictionary<string, string> commandDic;
            if (commandsTemplates.Templates.TryGetValue(commandName, out commandDic))
            {
                return commandDic.Keys;
            }
            else
                return null;

        }

        public static bool AddTemplate(this CommandsTemplates commandsTemplates, string commandName, string templateDescription, string commandLine, bool overwrite)
        {
            Dictionary<string, string> templates;
            if (commandsTemplates.Templates.TryGetValue(commandName, out templates))//command templates exists
            {
                if (templates.ContainsKey(templateDescription))
                {
                    if (overwrite)
                    {
                        templates[templateDescription] = commandLine;
                    }
                    else
                        return false;
                }
                else
                {
                    templates[templateDescription] = commandLine;
                }
            }
            else
            {
                templates = new Dictionary<string, string>();
                commandsTemplates.Templates[commandName] = templates;
                templates[templateDescription] = commandLine;
            }
            commandsTemplates.MarkDirty();
            return true;
        }

    }
}
