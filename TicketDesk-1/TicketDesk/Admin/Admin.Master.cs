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

namespace TicketDesk.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            UsersLinkContainer.Visible = (authenticationSection.Mode == AuthenticationMode.Forms);
            if(Page.Request.Path.ToUpperInvariant().Contains("DEFAULT.ASPX"))
            {
                AdminHomeLink.Font.Bold = true;
            }
            else if(Page.Request.Path.ToUpperInvariant().Contains("SETTINGS.ASPX"))
            {
                 SettingsLink.Font.Bold = true;
            }
            else if(Page.Request.Path.ToUpperInvariant().Contains("USERROLES.ASPX"))
            {
                UsersLink.Font.Bold = true;
            }
        }
    }
}
