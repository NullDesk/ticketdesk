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

namespace TicketDesk
{
    public partial class UserRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            AuthenticationSection authenticationSection = (AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication");

            if(authenticationSection.Mode != AuthenticationMode.Forms)
            {
                Page.Response.Redirect("~/", true);// this page can only be used with forms authentication. AD authentication assumes user managment happens outside the scope of the web application.
            }
            if(!Page.IsPostBack)
            {
                

                MembershipUserCollection uc = Membership.GetAllUsers();

                UserListBox.DataSource = Membership.GetAllUsers();
                UserListBox.DataBind();
            }
        }

        protected void UserListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserRolesList.Items.Clear();
            UserRolesPanel.Visible = true;
            ListItem li1 = new ListItem(ConfigurationManager.AppSettings["TicketSubmittersRoleName"]);
            li1.Selected = Roles.IsUserInRole(UserListBox.SelectedValue, li1.Value);
            ListItem li2 = new ListItem(ConfigurationManager.AppSettings["HelpDeskStaffRoleName"]);
            li2.Selected = Roles.IsUserInRole(UserListBox.SelectedValue, li2.Value);
            ListItem li3 = new ListItem(ConfigurationManager.AppSettings["AdministrativeRoleName"]);
            li3.Selected = Roles.IsUserInRole(UserListBox.SelectedValue, li3.Value);
            UserRolesList.Items.Add(li1);
            UserRolesList.Items.Add(li2);
            UserRolesList.Items.Add(li3);
        }

        protected void UserRolesList_SelectedIndexChanged(object sender, EventArgs e)
        {

            foreach(ListItem li in UserRolesList.Items)
            {
                if(li.Selected)
                {
                    if(!Roles.IsUserInRole(UserListBox.SelectedValue, li.Value))
                    {
                        if(!Roles.RoleExists(li.Value))
                        {
                            Roles.CreateRole(li.Value);
                        }
                        Roles.AddUserToRole(UserListBox.SelectedValue, li.Value);

                    }
                }
                else
                {
                    if(Roles.IsUserInRole(UserListBox.SelectedValue, li.Value))
                    {
                        Roles.RemoveUserFromRole(UserListBox.SelectedValue, li.Value);
                    }
                }
            }
        }


    }
}
