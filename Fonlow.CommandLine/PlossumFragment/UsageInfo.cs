/* Copyright (c) 2007  Peter Palotas
 *  
 *  This software is provided 'as-is', without any express or implied
 *  warranty. In no event will the authors be held liable for any damages
 *  arising from the use of this software.
 *  
 *  Permission is granted to anyone to use this software for any purpose,
 *  including commercial applications, and to alter it and redistribute it
 *  freely, subject to the following restrictions:
 *  
 *      1. The origin of this software must not be misrepresented; you must not
 *      claim that you wrote the original software. If you use this software
 *      in a product, an acknowledgment in the product documentation would be
 *      appreciated but is not required.
 *  
 *      2. Altered source versions must be plainly marked as such, and must not be
 *      misrepresented as being the original software.
 *  
 *      3. This notice may not be removed or altered from any source
 *      distribution.
 *  
 *  
 *  $Id: UsageInfo.cs 19 2007-08-15 13:14:32Z palotas $
 *  
 * 
 * Zijian had rewritten the codes of outputting option name, alias and description into 2 columns.
 * The new implementation is much shorter, and takes advantage of wider console window, rather than the default width 78.
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Plossum.CommandLine;
using System.Linq;

namespace Fonlow.CommandLine
{
    /// <summary>
    /// Represents the properties of a <see cref="CommandLineManagerAttribute"/> (or rather the object to which its 
    /// applied) that describe the command line syntax.
    /// </summary>
    public sealed class UsageInfo
    {
        internal UsageInfo(CommandLineParser parser)
        {
            this.parser = parser;
        }


        #region Public properties

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName
        {
            get { return parser.ApplicationName; }
        }

        /// <summary>
        /// Gets or sets the application version.
        /// </summary>
        /// <value>The application version.</value>
        public string ApplicationVersion
        {
            get { return parser.ApplicationVersion; }
        }

        /// <summary>
        /// Gets or sets the application copyright.
        /// </summary>
        /// <value>The application copyright.</value>
        public string ApplicationCopyright
        {
            get { return parser.ApplicationCopyright; }
        }

        /// <summary>
        /// Gets or sets the application description.
        /// </summary>
        /// <value>The application description.</value>
        public string ApplicationDescription
        {
            get { return parser.ApplicationDescription; }
        }

        #endregion

        static string CreateColumnsText(int indent, int column1Width, string c1, string c2)
        {
            int column2Width = Console.WindowWidth - indent - column1Width - 1;
            var wordsOfC1 = c1.Split(' ');
            var wordsOfC2 = c2.Split(' ');

            StringBuilder builder = new StringBuilder();
            List<string> list1 = new List<string>();
            foreach (var item in wordsOfC1)
            {
                if (builder.Length + item.Length + 1 >= column1Width)
                {
                    list1.Add(builder.ToString());
                    builder.Clear();
                }
                builder.Append(item + " ");
            }

            list1.Add(builder.ToString());

            builder.Clear();

            List<string> list2 = new List<string>();
            foreach (var item in wordsOfC2)
            {
                if (builder.Length + item.Length + 1 >= column2Width)
                {
                    list2.Add(builder.ToString());
                    builder.Clear();
                }
                builder.Append(item + " ");
            }

            list2.Add(builder.ToString());

            string format = String.Format("{{0,{0}}}{{1,-{1}}}{{2,-{2}}}", indent, column1Width, column2Width);

            var maxLines = Math.Max(list1.Count, list2.Count);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < maxLines; i++)
            {
                var column1Text = list1.Count > i ? list1[i] : String.Empty;
                var column2Text = list2.Count > i ? list2[i] : String.Empty;
                result.AppendLine(String.Format(format, " ", column1Text, column2Text));
            }

            return result.ToString();
        }


        #region Public methods


        /// <summary>
        /// Gets a string consisting of the program name, version and copyright notice. 
        /// </summary>
        /// <returns>a string consisting of the program name, version and copyright notice.</returns>
        /// <remarks>This string is suitable for printing as the first output of a console application.</remarks>
        public string GetHeaderAsString()
        {
            StringBuilder result = new StringBuilder();
            result.Append(ApplicationName ?? "Unnamed application");
            if (ApplicationVersion != null)
            {
                result.Append("  ");
                result.Append(Fonlow.CommandLineGui.Properties.Resources.Version);
                result.Append(' ');
                result.Append(ApplicationVersion);
            }
            result.AppendLine();

            if (ApplicationCopyright != null)
            {
                result.AppendLine(ApplicationCopyright);
            }
            return result.ToString();
        }

        /// <summary>
        /// Gets a string describing all the options of this option manager. Usable for displaying as a help 
        /// message to the user, provided that descriptions for all options and groups are provided.
        /// </summary>
        /// <returns>A string describing all the options of this option manager.</returns>
        public string GetOptionsAsString()
        {
            StringBuilder builder = new StringBuilder();
           
            if (parser.OptionAttributes.Length>0)
            {
                builder.AppendLine(CommandLineGui.Properties.Resources.Options);
                var optionAttributesGrouped = parser.OptionAttributes.GroupBy(d => d.GroupId).OrderBy(k=>k.Key);

                foreach (var optionsInGroup in optionAttributesGrouped)
                {
                    var groupId = optionsInGroup.Key;
                    if (!String.IsNullOrEmpty(groupId))
                    {
                        CommandLineOptionGroupAttribute groupAttribute = null;
                        parser.GroupAttributesDic.TryGetValue(groupId, out groupAttribute);

                        string groupName = groupAttribute == null ? null : groupAttribute.Name;
                        if (groupName != null)
                        {
                            builder.AppendLine(groupName + ":");
                        }
                    }

                    foreach (var option in optionsInGroup)
                    {
                        StringBuilder bb = new StringBuilder();
                        string s = parser.CommandLineManagerAttribute.OptionSeparator;
                        bb.Append(s + option.Name);
                        if (option.AliasesArray != null)
                        {
                            bb.Append(", ");
                            var aliasesWithSeparator = option.AliasesArray.Select(d => s + d).ToArray();
                            bb.Append(String.Join(", ", aliasesWithSeparator));
                        }

                        builder.Append(  CreateColumnsText(3, 20, bb.ToString(), option.Description));
                    }
                    
                }

            }

            return builder.ToString();
        }


        /// <summary>
        /// Converts this <see cref="UsageInfo"/> instance to a string.
        /// </summary>
        /// <returns>A string including the header, and a complete list of the options and their descriptions
        /// available in this <see cref="UsageInfo"/> object.</returns>
        public override  string ToString()
        {
            return ToString(false);
        }

        /// <summary>
        /// Converts this <see cref="UsageInfo"/> instance to a string.
        /// </summary>
        /// <param name="includeErrors">if set to <c>true</c> any errors that occured during parsing the command line will be included
        /// in the output.</param>
        /// <returns>A string including the header, optionally errors, and a complete list of the options and their descriptions
        /// available in this <see cref="UsageInfo"/> object.</returns>
        public string ToString(bool includeErrors)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine(GetHeaderAsString());
            result.AppendLine();

            result.AppendLine(GetOptionsAsString());
            result.AppendLine();

            if (includeErrors && parser.HasErrors)
            {
                result.Append("Errors: ");
                result.AppendLine(parser.ErrorMessage);
                
            }
            return result.ToString();
        }


        #endregion


        CommandLineParser parser;


    }
}
