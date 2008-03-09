using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;
using System.Collections.Generic;
using System.Text;
using TicketDesk.Engine;

namespace TicketDesk
{
    public partial class TicketSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TicketListControl.DataSourceID = "";
            if(!Page.IsPostBack)
            {
                StaffUserList.Items.AddRange(GetStaffUserList());
                SubmitterUserList.Items.AddRange(GetSubmitterUserList());
            }
            else
            {
                List<string> terms = new List<string>();
                List<string> andTerms = new List<string>();
                if(!string.IsNullOrEmpty(SearchTerms.Text))
                {
                    string[] words = SearchTerms.Text.Split(' ');

                    foreach(string word in words)
                    {
                        if(SearchTagsCheckBox.Checked)
                        {
                            terms.Add(string.Format("TagList.Contains(\"{0}\")", word));
                        }
                        if(SearchTitleCheckBox.Checked)
                        {
                            terms.Add(string.Format("Title.Contains(\"{0}\")", word));
                        }
                        if(SearchDetailsCheckBox.Checked)
                        {
                            terms.Add(string.Format("Details.Contains(\"{0}\")", word));
                        }
                    }

                    switch(StatusList.SelectedValue)
                    {
                        case "Open":
                            andTerms.Add("CurrentStatus != \"Closed\"");
                            break;
                        case "Active":
                            andTerms.Add("CurrentStatus == \"Active\"");
                            break;
                        case "More Info":
                            andTerms.Add("CurrentStatus == \"More Info\"");
                            break;
                        case "Resolved":
                            andTerms.Add("CurrentStatus == \"Resolved\"");
                            break;
                        case "Closed":
                            andTerms.Add("CurrentStatus == \"Closed\"");
                            break;
                        default:
                            //status DDL = any
                            //do nothing
                            break;
                    }

                    if(StaffUserList.SelectedValue != "any")
                    {
                            andTerms.Add(string.Format("AssignedTo == \"{0}\"", StaffUserList.SelectedValue));
                    }
                    if(SubmitterUserList.SelectedValue != "any")
                    {
                        andTerms.Add(string.Format("Owner == \"{0}\"", SubmitterUserList.SelectedValue));
                    }
                    if(CategoryList.SelectedValue != "any")
                    {
                        andTerms.Add(string.Format("Category == \"{0}\"", CategoryList.SelectedValue));
                    }
                    if(TypeList.SelectedValue != "any")
                    {
                        andTerms.Add(string.Format("Type == \"{0}\"", TypeList.SelectedValue));
                    }

                }
                if(terms.Count > 0)
                {
                    TicketSearchResults.Visible = true;
                    string s = string.Format("({0})", string.Join(" || ", terms.ToArray()));
                    string a = string.Empty;
                    if(andTerms.Count > 0)
                    {
                        a = string.Format(" && ({0})", string.Join(" && ", andTerms.ToArray()));
                    }
                    SearchTicketsLinqDataSource.Where = s + a;
                    TicketListControl.DataSourceID = "SearchTicketsLinqDataSource";

                    TicketListControl.ShowList();
                }
                else
                {
                    TicketSearchResults.Visible = false;

                }
            }
        }

        protected void SearchNow_Click(object sender, EventArgs e)
        {

        }

        public ListItem[] GetStaffUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetUsersInRoleType("HelpDeskStaffRoleName");
            foreach(User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));
            }
            return returnUsers.ToArray();
        }


        public ListItem[] GetSubmitterUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetUsersInRoleType("TicketSubmittersRoleName");
            foreach(User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));

            }
            return returnUsers.ToArray();
        }
    }
}
