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
using System.Web.Security;
using System.Web.UI;

namespace TicketDesk
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            if(authenticationSection.Mode != AuthenticationMode.Forms)
            {
                Page.Response.Redirect("~/", true);// this page can only be used with forms authentication. AD authentication assumes user managment happens outside the scope of the web application.
            }
        }

        protected void CreateUserForm_CreatedUser(object sender, EventArgs e)
        {
            if(Convert.ToBoolean(ConfigurationManager.AppSettings["CreateSqlMembershipRegistrationsAsSubmitters"]))
            {
                string defaultRole = ConfigurationManager.AppSettings["TicketSubmittersRoleName"];
                if(!Roles.RoleExists(defaultRole))
                {
                    Roles.CreateRole(defaultRole);
                }
                Roles.AddUserToRole(CreateUserForm.UserName, defaultRole);
            }
            FormsAuthentication.RedirectFromLoginPage(CreateUserForm.UserName,false);
        }

        
    }
}
