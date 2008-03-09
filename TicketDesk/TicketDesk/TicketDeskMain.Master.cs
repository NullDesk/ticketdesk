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
using TicketDesk.Engine;
using System.Web.Configuration;

namespace TicketDesk
{
    public partial class TicketDeskMain : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Page.User.Identity.IsAuthenticated)
            {
                AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

                UserDisplayNameLabel.Text = Page.User.Identity.GetUserDisplayName();
                FormsStatusDisplayPanel.Visible = (authenticationSection.Mode == AuthenticationMode.Forms);
                UserRolesDisplayPanel.Visible = (authenticationSection.Mode == AuthenticationMode.Forms) && (SecurityManager.IsAdmin);
                LoginStatusControl.Visible = (authenticationSection.Mode == AuthenticationMode.Forms);
            }
            else
            {
                WelcomeBox.Visible = false;
            }
            if(Page.Request.Path.ToUpperInvariant().Contains("TICKETCENTER.ASPX"))
            {
                TicketCenterLink.Font.Bold = true;
            }
            else if(Page.Request.Path.ToUpperInvariant().Contains("MYTICKETS.ASPX"))
            {
                MyTicketsLink.Font.Bold = true;
            }
            else if(Page.Request.Path.ToUpperInvariant().Contains("TICKETSEARCH.ASPX"))
            {
                TicketSearchLink.Font.Bold = true;
            }
            else if(Page.Request.Path.ToUpperInvariant().Contains("NEWTICKET.ASPX"))
            {
                NewTicketMenuLink.Font.Bold = true;
            }
        }
    }
}
