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
using System.Configuration;
using System.Web.Configuration;
using System.Web.UI;
using TicketDesk.Engine;

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
