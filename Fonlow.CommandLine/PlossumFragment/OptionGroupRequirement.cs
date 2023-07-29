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
 *  $Id: OptionGroupRequirement.cs 19 2007-08-15 13:14:32Z palotas $
 */
 using System;
using System.Collections.Generic;
using System.Text;

namespace Plossum.CommandLine
{
    /// <summary>
    /// Specifies the requirements on an option group.
    /// </summary>
    public enum OptionGroupRequirement
    {
        /// <summary>
        /// Indicates that no requirement is placed on the options of this group.
        /// </summary>
        None,
        /// <summary>
        /// Indicates that at most one of the options in this group may be specified on the command line.
        /// </summary>
        AtMostOne,
        /// <summary>
        /// Indicates that at least one of the options in this group may be specified on the command line.
        /// </summary>
        AtLeastOne,
        /// <summary>
        /// Indicates that exactly one of the options in this group must be specified on the command line.
        /// </summary>
        ExactlyOne,
        /// <summary>
        /// Indicates that all options of this group must be specified.
        /// </summary>
        All        
    }
}
