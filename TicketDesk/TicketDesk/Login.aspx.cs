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
using System.Web.Profile;
using System.Web.Configuration;

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
