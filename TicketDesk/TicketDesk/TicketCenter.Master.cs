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
using System;

namespace TicketDesk
{
    public partial class TicketCenter1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string viewMode = Request.QueryString["View"] ?? "unassigned";
            switch(viewMode)
            {
                case "unassigned":
                    UnassignedTicketsMenuLink.Font.Bold = true;
                    break;
                case "assigned":
                    AssignedTicketsMenuLink.Font.Bold = true;
                    break;
                case "status":
                    StatusTicketsMenuLink.Font.Bold = true;
                    break;
                //case "resolved":
                //    ResolvedTicketsMenuLink.Font.Bold = true;
                //    break;
                //case "closed":
                //    ClosedTicketsMenuLink.Font.Bold = true;
                //    break;
                case "tagsandcategories":
                    TagsTicketMenuLink.Font.Bold = true;
                    break;
            }
        }
    }
}
