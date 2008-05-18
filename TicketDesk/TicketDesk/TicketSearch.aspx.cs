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
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine;

namespace TicketDesk
{
    public partial class TicketSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TicketListControl.DataSourceID = "SearchTicketsLinqDataSource";
            SortEditor.TicketListControl = TicketListControl;
            if(!Page.IsPostBack)
            {
                StaffUserList.Items.AddRange(GetStaffUserList());
                SubmitterUserList.Items.AddRange(GetSubmitterUserList());
                CategoryList.DataSource = SettingsManager.CategoriesList;
                CategoryList.DataBind();
                TypeList.DataSource = SettingsManager.TicketTypesList;
                TypeList.DataBind();

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
            User[] users = SecurityManager.GetHelpDeskUsers();
            foreach(User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));
            }
            return returnUsers.ToArray();
        }


        public ListItem[] GetSubmitterUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetHelpDeskUsers();
            foreach(User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));

            }
            return returnUsers.ToArray();
        }
    }
}
