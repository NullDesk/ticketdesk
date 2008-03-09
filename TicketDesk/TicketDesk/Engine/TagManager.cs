using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
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
