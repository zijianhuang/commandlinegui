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
 *  $Id: CommandLineOptionGroupAttribute.cs 19 2007-08-15 13:14:32Z palotas $
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace Plossum.CommandLine
{
    /// <summary>
    /// Attribute used to specify an option group in a command line option manager object.
    /// </summary>
    /// <remarks>Option groups are used for logical grouping of options. This is in part useful for 
    /// grouping related options in the usage documentation generated, but also to place certain 
    /// restrictions on a group of options.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public sealed class CommandLineOptionGroupAttribute : System.Attribute
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineOptionGroupAttribute"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <remarks>The id must be unique within the option manager object.</remarks>
        public CommandLineOptionGroupAttribute(string id)
        {
            mId = id;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>This is the name that will be displayed as a headline for the options contained in the
        /// group in any generated documentation. If not explicitly set it will be the same as <see cref="Id"/>.</remarks>
        public string Name
        {
            get { return mName; }
            set { mName = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return mDescription; }
            set { mDescription = value; }
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public string Id
        {
            get { return mId; }
        }

        /// <summary>
        /// Gets or sets the requirements placed on the options in this group.
        /// </summary>
        /// <value>requirements placed on the options in this group.</value>
         public OptionGroupRequirement Require
        {
            get { return mRequired; }
            set { mRequired = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether explicit assignment is required for the options
        /// of this group.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if explicit assignment is required by the options of this group; otherwise, <c>false</c>.        
        /// </value>
        /// <remarks>This defaults all options in this group to the specified value, but setting another value 
        /// explicitly on an option overrides this setting.</remarks>
         public bool RequireExplicitAssignment
         {
             get { return requireExplicitAssignment; }
             set { requireExplicitAssignment = value; RequireExplicitAssignmentAssigned = true; }
         }

         bool requireExplicitAssignment;

         public bool RequireExplicitAssignmentAssigned { get; private set; }
        
        #endregion



        #region Private fields

        private string mId;
        private string mName;
        private string mDescription;
        private OptionGroupRequirement mRequired;

        #endregion
    }
}
