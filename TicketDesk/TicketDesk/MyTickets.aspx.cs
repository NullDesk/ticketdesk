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
using System.Web.UI;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;

namespace TicketDesk
{
    public partial class MyTickets : System.Web.UI.Page
    {

        TicketDataDataContext ctx = new TicketDataDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

            AssignedSubMenuCell.Visible = SecurityManager.IsStaffOrAdmin;
            OwnedSubMenuCell.Visible = SecurityManager.IsStaffOrAdmin;


            string viewMode = Request.QueryString["View"] ?? "open";
            string user = Page.User.Identity.GetFormattedUserName();
            switch(viewMode)
            {
                case "open":
                    Page.Title = "My Tickets - Open";
                    OpenTicketsMenuLink.Font.Bold = true;
                    MyOpenTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MyOpenTicketsLinqDataSource";
                    
                    break;
                case "owned":
                    Page.Title = "My Tickets - Owned by Me";
                    OwnedTicketsMenuLink.Font.Bold = true;
                    MySubmittedTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MySubmittedTicketsLinqDataSource";
                    break;
                case "assigned":
                    Page.Title = "My Tickets - Assigned to Me";
                    AssignedTicketsMenuLink.Font.Bold = true;
                    MyAssignedTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MyAssignedTicketsLinqDataSource";
                    break;
                case "all":
                    Page.Title = "My Tickets - All My Tickets";
                    AllTicketsMenuLink.Font.Bold = true;
                    MyAllTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MyAllTicketsLinqDataSource";
                    break;
                default:
                    //do nothing
                    break;
            }
            TicketListControl.DataBind();
        }
    }
}
