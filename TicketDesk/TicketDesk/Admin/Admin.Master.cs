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
using System.Web.Configuration;

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
