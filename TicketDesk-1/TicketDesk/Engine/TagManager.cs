// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System.Collections.Generic;

namespace TicketDesk.Engine
{
    /// <summary>
    /// Provides common operations related to ticket tagging.
    /// </summary>
    public class TagManager
    {
        /// <summary>
        /// Gets the tags from a string of text. Duplicate tags are removed, and tags converted to lower case.
        /// </summary>
        /// <returns></returns>
        public static string[] GetTagsFromString(string tagString)
        {
            
            List<string> returnTags = new List<string>();
            string[] tags = tagString.Split(',');
            foreach(string t in tags)
            {
                var formattedTag = t.ToLowerInvariant().Trim();
                if(!string.IsNullOrEmpty(formattedTag) && !returnTags.Contains(formattedTag))
                {
                    returnTags.Add(formattedTag);
                }
            }
            return returnTags.ToArray();
        }

    }
}
