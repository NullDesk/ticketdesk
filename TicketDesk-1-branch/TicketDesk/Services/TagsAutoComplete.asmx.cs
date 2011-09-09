using System.Collections.Generic;
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Services;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Services
{
    /// <summary>
    /// Summary description for TagsAutoComplete
    /// </summary>
    [WebService(Namespace = "http://ticketdesk.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class TagsAutoComplete : System.Web.Services.WebService
    {

        

        [WebMethod]
        public string[] GetTagCompletionList(string prefixText, int count)
        {
            List<string> returnList = new List<string>();
            if(count == 0)
            {
                count = 10;
            }
            string[] tags = prefixText.Replace(" ,", ",").Replace(", ", ",").Split(',');//eliminate extra spaces around commas before split
            string textToCheck = tags[tags.Length - 1];//last element
            if(textToCheck.Trim().Length > 1)//only check if user has typed more than 1 character for the last item of the taglist
            {
                string fixedText = string.Empty;//all that stuff the user typed before the last comma in the text
                if(tags.Length > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    for(int x = 0; x < tags.Length - 1; x++)
                    {
                        sb.Append(tags[x] + ",");
                    }
                    fixedText = sb.ToString();
                }

                TicketDataDataContext context = new TicketDataDataContext();
                var distinctTagsQuery = (
                            from tag in context.TicketTags
                            orderby tag.TagName
                            where tag.TagName.StartsWith(textToCheck)
                            select tag.TagName).Distinct().Take(count);

                
                foreach(var distinctTag in distinctTagsQuery)
                {
                    if(tags.Count(t => t.ToUpperInvariant() == distinctTag.ToUpperInvariant()) < 1)//eliminate any tags that were already used (that are in the fixedText).
                    {
                        returnList.Add(fixedText + distinctTag);//append the other items in the list plus the new tag possibilitiy
                    }
                }
            }
            return returnList.ToArray();


        }
    }
}
