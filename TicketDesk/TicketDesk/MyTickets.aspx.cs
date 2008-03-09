using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;

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
                    OpenTicketsMenuLink.Font.Bold = true;
                    MyOpenTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MyOpenTicketsLinqDataSource";
                    
                    break;
                case "owned":
                    OwnedTicketsMenuLink.Font.Bold = true;
                    MySubmittedTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MySubmittedTicketsLinqDataSource";
                    break;
                case "assigned":
                    AssignedTicketsMenuLink.Font.Bold = true;
                    MyAssignedTicketsLinqDataSource.WhereParameters["UserName"].DefaultValue = Page.User.Identity.GetFormattedUserName();
                    TicketListControl.DataSourceID = "MyAssignedTicketsLinqDataSource";
                    break;
                case "all":
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
