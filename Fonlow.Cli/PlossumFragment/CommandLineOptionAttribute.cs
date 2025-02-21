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
 *  $Id: CommandLineOptionAttribute.cs 19 2007-08-15 13:14:32Z palotas $
 *  
 * 
 */
using System;

namespace Plossum.CommandLine
{
    /// <summary>
    /// Attribute indicating that the field to which is applied should receive the value of a command line option, 
    /// and also specifies the properties of that option.
    /// </summary>
    /// <remarks>This attribute may be applied to properties, fields and methods. If applied to a method, the method
    /// must accept exactly one argument which may not be an array or collection type. The method will then be called 
    /// once every time the option is specified with the value of the option.  If specified on a field or property
    /// the type of that member must be one of the built-in types, a one-dimensional array of one of the 
    /// built in types, a <see cref="System.Collections.ICollection"/> or a 
    /// <see cref="T:System.Collections.Generic.ICollection`1"/>. If an array type or a collection type, the enumerable 
    /// will contain all values that the option is specified with (if specified multiple times).</remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, AllowMultiple=true)]
    public sealed class CommandLineOptionAttribute : System.Attribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineOptionAttribute"/> class.
        /// </summary>
        public CommandLineOptionAttribute()
        {
            RequireExplicitAssignment = true;//only apply to non-bool
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the group to which this option belongs.
        /// </summary>
        /// <value>The id of the group to which this option belongs, or null if this option does not belong to any group.</value>
        public string GroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the names of other options that must not be specified on the command line if this option is specified.
        /// </summary>
        /// <value>The names of other options that must not be specified on the command line if this option is specified.</value>
        /// <remarks>Option names can be comma- or space-separated.  If an option prohibits another option, this 
        /// automatically means that the other option also prohibits the first option, and it need not be 
        /// explicitly specified.</remarks>
        /// <seealso cref="CommandLineOptionGroupAttribute.Require"/>
        public string Prohibits
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of this option.
        /// </summary>
        /// <value>The name of this option.</value>
        /// <remarks>This is the text that the user will specify on the command line. If not explicitly set, this 
        /// will take on the name of the member to which the attribute was applied.</remarks>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the maximum number of times this option may be specified on the command line.
        /// </summary>
        /// <value>The the maximum number of times this option may be specified on the command line.</value>
        /// <remarks><para>The default value for this option is 1 for any non-collection or array type, and 0 otherwise.</para>
        /// <para>If this value is set to zero that means that there is no upper bound on the number of times
        /// this option may be specified.</para>
        /// <note>Note that this value must be either 0 or 1 if the member is not a method, or if the field or 
        /// property does not represent an array or collection type.</note></remarks>
        public int MaxOccurs
        {
            get { return maxOccurs; }
            set { maxOccurs = value; IsMaxOccursSet = true; }
        }

        int maxOccurs;

        /// <summary>
        /// Gets or sets the minimum number of times this option may be specified on the command line.
        /// </summary>
        /// <value>The the minimum number of times this option may be specified on the command line.</value>
        /// <remarks>
        ///     <para>The default value for this option is 0.</para>
        ///     <note>This value must be less than or equal to <see cref="MaxOccurs"/>, unless <see cref="MaxOccurs"/> is 
        ///         equal to 0.</note>
        /// </remarks>
        public int MinOccurs
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default value for this option.
        /// </summary>
        public object DefaultAssignmentValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this option requires an explicit assignment character or not.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this option requires an explicit assignment character; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>Requiring an explicit assignment character means that a valid assignment character must follow
        /// the option name on the command line to assign a value to this option. If this value is not set to true
        /// the assignment character between an option name and its intended value may be omitted. 
        /// 
        /// This setting is effective
        /// only for Command Line GUI to generate command line text, however when parsing, this setting is ignored.</remarks>
        public bool RequireExplicitAssignment
        {
            get { return requireExplicitAssignment; }
            set { requireExplicitAssignment = value; RequireExplicitAssignmentAssigned = true; }
        }

        bool requireExplicitAssignment;

        public bool RequireExplicitAssignmentAssigned { get; private set; }

        string alias;
        /// <summary>
        /// Gets or sets the aliases of this option.
        /// </summary>
        /// <value>A comma- or space-separated list of the aliases for this option.</value>
        /// <remarks>An option may have one or more aliases, which are other names with which to refer to the 
        /// same option. This is common in the UN*X world, where many programs provide both a long and a short name
        /// for most options.</remarks>
        public string Aliases
        {
            get { return alias; }
            set
            {
                alias = value;
                AliasesArray = alias ==null? null : alias.Split(',', ' ');
            }
        }

        public string[] AliasesArray { get; private set; }

        /// <summary>
        /// Gets or sets the maximum value for a numeric option.
        /// </summary>
        /// <value>The maximum value for a numeric option.</value>
        /// <remarks>This option should be null for any non-numeric option. This option also defaults to null for non-numeric
        /// options, or the maximum value for the type of this option for numerical options.</remarks>
        public object MaxValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the minimum value for a numeric option.
        /// </summary>
        /// <value>The minimum value for a numeric option.</value>
        /// <remarks>This option should be null for any non-numeric option. This option also defaults to null for non-numeric
        /// options, or the minimum value for the type of this option for numerical options.</remarks>
        public object MinValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether this instance has its <see cref="MaxOccurs"/> property explicitly set.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has its <see cref="MaxOccurs"/> property explicitly set.; otherwise, <c>false</c>.
        /// </value>
        internal bool IsMaxOccursSet
        {
            get;
            private set;
        }

        #endregion


    }
    
}
