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
using System.Web.UI.HtmlControls;

namespace TicketDesk
{
    public partial class TicketDeskMain : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

                UserDisplayNameLabel.Text = Page.User.Identity.GetUserDisplayName();
                FormsStatusDisplayPanel.Visible = (authenticationSection.Mode == AuthenticationMode.Forms);
                UserRolesDisplayPanel.Visible = (SecurityManager.IsAdmin);
                LoginStatusControl.Visible = (authenticationSection.Mode == AuthenticationMode.Forms);
            }
            else
            {
                WelcomeBox.Visible = false;
            }
            if (Page.Request.Path.ToUpperInvariant().Contains("TICKETCENTER.ASPX"))
            {
                TicketCenterLink.Font.Bold = true;
            }

            else if (Page.Request.Path.ToUpperInvariant().Contains("TICKETSEARCH.ASPX"))
            {
                TicketSearchLink.Font.Bold = true;
            }
            else if (Page.Request.Path.ToUpperInvariant().Contains("NEWTICKET.ASPX"))
            {
                NewTicketMenuLink.Font.Bold = true;
            }

            bool isEnabled = false;
            string enabledString = ConfigurationManager.AppSettings["EnableRSS"];

            if (string.IsNullOrEmpty(enabledString))
            {
                isEnabled = Convert.ToBoolean(enabledString);
            }


            if (isEnabled)
            {

                //make default feeds
                var rlink = new HtmlLink();
                rlink.Href = Page.ResolveClientUrl("~/Services/Rss.svc/createfeed/");
                rlink.Attributes.Add("rel", "alternate");
                rlink.Attributes.Add("type", "application/rss+xml");
                rlink.Attributes.Add("title", "All Tickets - RSS 2.0");
                Page.Header.Controls.Add(rlink);

                if (SecurityManager.IsStaff)
                {
                    var q = string.Format("?assigned={0}", Page.User.Identity.GetFormattedUserName());
                    var rlink2 = new HtmlLink();
                    rlink2.Href = Page.ResolveClientUrl("~/Services/Rss.svc/createfeed/" + q);
                    rlink2.Attributes.Add("rel", "alternate");
                    rlink2.Attributes.Add("type", "application/rss+xml");
                    rlink2.Attributes.Add("title", "Tickets Assigned to Me - RSS 2.0");
                    Page.Header.Controls.Add(rlink2);
                }

                if (SecurityManager.IsTicketSubmitter)
                {
                    var q = string.Format("?owner={0}", Page.User.Identity.GetFormattedUserName());
                    var rlink2 = new HtmlLink();
                    rlink2.Href = Page.ResolveClientUrl("~/Services/Rss.svc/createfeed/" + q);
                    rlink2.Attributes.Add("rel", "alternate");
                    rlink2.Attributes.Add("type", "application/rss+xml");
                    rlink2.Attributes.Add("title", "Tickets Owned by Me - RSS 2.0");
                    Page.Header.Controls.Add(rlink2);

                }
            }

        }
    }
}
