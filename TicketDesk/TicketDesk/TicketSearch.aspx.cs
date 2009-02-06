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

            bool isSearchQueryString = false;
            if (!Page.IsPostBack)
            {
                StaffUserList.Items.AddRange(GetStaffUserList());
                SubmitterUserList.Items.AddRange(GetSubmitterUserList());
                CategoryList.DataSource = SettingsManager.CategoriesList;
                CategoryList.DataBind();
                BuildPriorityList();
                TypeList.DataSource = SettingsManager.TicketTypesList;
                TypeList.DataBind();
                isSearchQueryString = ProcessSearchQuery();
            }

             
            if (!Page.IsPostBack && !isSearchQueryString)
            {
                SearchTitleCheckBox.Checked = true;
            }
            if (Page.IsPostBack || isSearchQueryString)
            {
                List<string> terms = new List<string>();
                List<string> andTerms = new List<string>();
                if (!string.IsNullOrEmpty(SearchTerms.Text))
                {
                    string[] words = SearchTerms.Text.Split(' ');

                    foreach (string word in words)
                    {
                        if (SearchTagsCheckBox.Checked)
                        {
                            terms.Add(string.Format("TagList.Contains(\"{0}\")", word));
                        }
                        if (SearchTitleCheckBox.Checked)
                        {
                            terms.Add(string.Format("Title.Contains(\"{0}\")", word));
                        }
                        if (SearchDetailsCheckBox.Checked)
                        {
                            terms.Add(string.Format("Details.Contains(\"{0}\")", word));
                        }
                    }
                }
                switch (StatusList.SelectedValue)
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

                if (StaffUserList.SelectedValue != "any")
                {
                    andTerms.Add(string.Format("AssignedTo == \"{0}\"", StaffUserList.SelectedValue));
                }
                if (SubmitterUserList.SelectedValue != "any")
                {
                    andTerms.Add(string.Format("Owner == \"{0}\"", SubmitterUserList.SelectedValue));
                }
                if (CategoryList.SelectedValue != "any")
                {
                    andTerms.Add(string.Format("Category == \"{0}\"", CategoryList.SelectedValue));
                }
                if (TypeList.SelectedValue != "any")
                {
                    andTerms.Add(string.Format("Type == \"{0}\"", TypeList.SelectedValue));
                }

                if (PriorityList.SelectedValue != "any")
                {
                    andTerms.Add(string.Format("Priority == \"{0}\"", PriorityList.SelectedValue));
                }



                ListViewControl.Visible = true;

                string s = string.Empty;
                if (terms.Count > 0)
                {

                    s = string.Format("({0})", string.Join(" || ", terms.ToArray()));
                }
                string a = string.Empty;
                if (andTerms.Count > 0)
                {
                    string seperator = ((terms.Count > 0) ? " && " : string.Empty);
                    a = string.Format("{1}({0})", string.Join(" && ", andTerms.ToArray()), seperator);
                }
                if (!string.IsNullOrEmpty(s) || !string.IsNullOrEmpty(a))
                {
                    ListViewControl.Where = s + a;
                }



            }
        }

        private bool ProcessSearchQuery()
        {
            bool valildSearch = false;
            string searchTitle = Page.Request.QueryString["title"];
            string searchDetail = Page.Request.QueryString["detail"];
            string searchTag = Page.Request.QueryString["tag"];

            if (!string.IsNullOrEmpty(searchTitle))
            {
                valildSearch = true;
                SearchTitleCheckBox.Checked = true;
                if (string.IsNullOrEmpty(SearchTerms.Text))
                {
                    SearchTerms.Text = searchTitle;
                }
                else
                {
                    SearchTerms.Text = "," + searchTitle;
                }
            }

            if (!string.IsNullOrEmpty(searchDetail))
            {
                valildSearch = true;
                SearchDetailsCheckBox.Checked = true;
                if (string.IsNullOrEmpty(SearchTerms.Text))
                {
                    SearchTerms.Text = searchDetail;
                }
                else
                {
                    SearchTerms.Text = "," + searchDetail;
                }
            }

            if (!string.IsNullOrEmpty(searchTag))
            {
                valildSearch = true;
                SearchTagsCheckBox.Checked = true;
                if (string.IsNullOrEmpty(SearchTerms.Text))
                {
                    SearchTerms.Text = searchTag;
                }
                else
                {
                    SearchTerms.Text = "," + searchTag;
                }
            }

            SearchTerms.Text = SearchTerms.Text.TrimStart(',').TrimEnd(',');

            string searchStatus = Page.Request.QueryString["status"];

            if (!string.IsNullOrEmpty(searchStatus))
            {
                ListItem li = StatusList.Items.FindByValue(searchStatus);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }


            string searchCategory = Page.Request.QueryString["cat"];

            if (!string.IsNullOrEmpty(searchCategory))
            {
                ListItem li = CategoryList.Items.FindByValue(searchCategory);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }

            string searchType = Page.Request.QueryString["type"];

            if (!string.IsNullOrEmpty(searchType))
            {
                ListItem li = TypeList.Items.FindByValue(searchType);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }

            string searchAssignedTo = Page.Request.QueryString["assign"];

            if (!string.IsNullOrEmpty(searchAssignedTo))
            {
                ListItem li = StaffUserList.Items.FindByValue(searchAssignedTo);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }

            string searchOwner = Page.Request.QueryString["owner"];

            if (!string.IsNullOrEmpty(searchOwner))
            {
                ListItem li = SubmitterUserList.Items.FindByValue(searchOwner);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }

            string searchPriority = Page.Request.QueryString["priority"];

            if (!string.IsNullOrEmpty(searchPriority))
            {
                ListItem li = PriorityList.Items.FindByValue(searchPriority);
                if (li != null)
                {
                    valildSearch = true;
                    li.Selected = true;
                }
            }

            if (valildSearch && string.IsNullOrEmpty(searchStatus))// if no status selected in query string, but it is a query string then search only open tickets by default
            {
                ListItem li = StatusList.Items.FindByValue("Open");
                if (li != null)
                {
                    li.Selected = true;
                }
            }

            return valildSearch;


        }

        protected void SearchNow_Click(object sender, EventArgs e)
        {
            Bind();
        }

        private void Bind()
        {
            ListViewControl.Bind();
        }



        public ListItem[] GetStaffUserList()
        {

            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetHelpDeskUsers();
            foreach (User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));
            }
            return returnUsers.ToArray();
        }


        public ListItem[] GetSubmitterUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetTicketSubmitterUsers();
            foreach (User user in users)
            {
                returnUsers.Add(new ListItem(user.DisplayName, user.Name));

            }
            return returnUsers.ToArray();
        }


        private void BuildPriorityList()
        {
            PriorityList.SelectedIndex = -1;
            //PriorityList.Items.Clear();
            //PriorityList.Items.Add(string.Empty);
            string[] priorities = SettingsManager.PrioritiesList;
            foreach (string p in priorities)
            {
                ListItem priItem = new ListItem(p);
                PriorityList.Items.Add(priItem);
            }
        }

    }
}
