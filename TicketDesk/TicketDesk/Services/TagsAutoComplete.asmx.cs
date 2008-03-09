using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using TicketDesk.Engine.Linq;
using System.Text;

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
