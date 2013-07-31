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
    public partial class MyAccount : System.Web.UI.Page
    {

        private MembershipUser user = Membership.GetUser();

        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordMessage.Text = string.Empty;
            DetailsMessage.Text = string.Empty;
            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            if(authenticationSection.Mode != AuthenticationMode.Forms)
            {
                Page.Response.Redirect("~/", true);// this page can only be used with forms authentication. AD authentication assumes user managment happens outside the scope of the web application.
            }
            if(!Page.IsPostBack)
            {
                DisplayName.Focus();
                Email.Text = user.Email;
                DisplayName.Text = user.Comment;
            }
        }

        protected void SaveDetails_Click(object sender, EventArgs e)
        {
            
            user.Email = Email.Text;
            user.Comment = DisplayName.Text;
            Membership.UpdateUser(user);
            DetailsMessage.Text = "Your account has been updated!";
        }

        protected void SavePassword_Click(object sender, EventArgs e)
        {
            if(user.ChangePassword(OldPassword.Text, Password.Text))
            {
                PasswordMessage.Text = "Password has been changed!";
            }
            else
            {
                OldPassword.Focus();
                PasswordMessage.Text = "Unable to change password, please check to ensure you entered your old password correctly and try again.";
            }
        }
    }
}
