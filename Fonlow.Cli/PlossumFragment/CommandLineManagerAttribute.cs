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
 *  $Id: CommandLineManagerAttribute.cs 19 2007-08-15 13:14:32Z palotas $
 */
using System;
using System.Reflection;

namespace Plossum.CommandLine
{
    /// <summary>
    /// Attribute describing the command line manager class that will be used for storing the values
    /// of the command line options parsed by the CommandLineParser.
    /// </summary>
    /// <remarks>This attribute is required for any class that is to act as a command line manager for parsing. It
    /// may only be specified on a class and only one occurence may be specified.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
    public sealed class CommandLineManagerAttribute : System.Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineManagerAttribute"/> class.
        /// </summary>
        public CommandLineManagerAttribute()
        {
            OptionSeparator = "/";
            Assignment = ":";
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
            {
                return;
            }

            AssemblyName name = assembly.GetName();
            Version = name.Version.ToString();

            foreach (object objAttribute in assembly.GetCustomAttributes(false))
            {
                AssemblyCopyrightAttribute copyrightAttribute = objAttribute as AssemblyCopyrightAttribute;
                if (copyrightAttribute != null)
                {
                    Copyright = copyrightAttribute.Copyright;
                    continue;
                }
                
                AssemblyTitleAttribute titleAttribute = objAttribute as AssemblyTitleAttribute;
                if (titleAttribute != null)
                {
                    ApplicationName = titleAttribute.Title;
                    continue;
                }

                AssemblyDescriptionAttribute descriptionAttribute = objAttribute as AssemblyDescriptionAttribute;
                if (descriptionAttribute != null)
                {
                    Description = descriptionAttribute.Description;
                    continue;
                }                
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        /// <remarks>If not explicitly specified, this value will be retrieved from the assembly information.</remarks>
        public string ApplicationName
        {
            get { return mApplicationName; }
            set { mApplicationName = value; }
        }

        /// <summary>
        /// Gets or sets the copyright message.
        /// </summary>
        /// <value>The copyright message.</value>
        /// <remarks>If not explicitly specified, this value will be retrieved from the assembly information.</remarks>
        public string Copyright
        {
            get { return mCopyright; }
            set { mCopyright = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        /// <remarks>If not explicitly specified, the application will be retrieved from the assembly information.</remarks>
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        /// <summary>
        /// Gets or sets the version of this application.
        /// </summary>
        /// <value>The version of this application.</value>
        /// <remarks>If not explicitly specified, the application will be retrieved from the assembly information.</remarks>
        public string Version
        {
            get { return mVersion; }
            set { mVersion = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether options are case sensitive or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if options are case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseSensitive
        {
            get { return mIsCaseSensitive; }
            set { mIsCaseSensitive = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether options in this manager requires explicit assignment or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if options require explicit assignment; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Explicit assignment means that the assignment character must be used to assign a value
        /// to an option. If set to false a space character after an option followed by a value will be interpreted
        /// as an assignment.
        /// <note>This value sets the default value for all options contained in this manager, but may be overridden for
        /// individual groups.</note></remarks>
        public bool RequireExplicitAssignment
        {
            get { return requireExplicitAssignment; }
            set { requireExplicitAssignment = value; RequireExplicitAssignmentAssigned = true; }
        }

        bool requireExplicitAssignment;

        public bool RequireExplicitAssignmentAssigned { get; private set; }

        /// <summary>
        /// Option separator in front of each option name.
        /// </summary>
        public string OptionSeparator { get; set; }

        /// <summary>
        /// Assignment symbol to be used between option name and values. The default is : 
        /// </summary>
        public string Assignment { get; set; }
	
        #endregion

        #region Private fields

        private string mCopyright;
        private string mApplicationName;
        private string mDescription;
        private bool mIsCaseSensitive;
        private string mVersion;

        #endregion
    }
}