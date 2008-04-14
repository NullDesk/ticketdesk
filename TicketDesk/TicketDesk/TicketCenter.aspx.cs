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
using TicketDesk.Engine.Linq;
using AjaxControlToolkit;
using System.Web.UI.HtmlControls;

namespace TicketDesk
{
    public partial class TicketCenter : System.Web.UI.Page
    {
      
        TicketDataDataContext ctx = new TicketDataDataContext();

        string viewMode = null;
        string tagName = null;
        string category = null;
        string status = null;
        string assignedTo = null;
        string ctrlname = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            SortEditor.TicketListControl = TicketListControl;
            viewMode = Request.QueryString["View"] ?? "unassigned";
            status = Request.QueryString["Status"] ?? "active";
            tagName = Request.QueryString["TagName"];
            category = Request.QueryString["Category"];
            assignedTo = Request.QueryString["User"] ?? "all";
            ctrlname = Page.Request.Params["__EVENTTARGET"];
            string user = Page.User.Identity.GetFormattedUserName();
            bool bindList = true;
            switch(viewMode)
            {
                case "unassigned":
                    TicketListControl.DataSourceID = "UnassignedTicketsLinqDataSource";
                    break;
                case "assigned":
                    // Don't bind if postback was from assigned to ddl. 
                    //  We skip loading data, the list's selected idx chng handler will just be redirecting the browser anyway
                    bindList = (ctrlname != StaffUserList.UniqueID);
                    if(bindList)
                    {
                        BuildStaffList(true, false);

                        if(assignedTo != "all")
                        {
                            AssignedTicketsLinqDataSource.Where = string.Format("(AssignedTo == \"{0}\") && (CurrentStatus != @CurrentStatus)", assignedTo);
                        }

                        TicketListControl.DataSourceID = "AssignedTicketsLinqDataSource";
                    }
                    break;
                case "status":
                    // Don't bind if postback was from assigned to ddl. 
                    //  We skip loading data, the list's selected idx chng handler will just be redirecting the browser anyway
                    bindList = (ctrlname != StaffUserList.UniqueID && ctrlname != StatusList.UniqueID);
                    if(bindList)
                    {
                       
                        BuildStaffList(true, true);
                        BuildStatusList();
                        string currentStatusWherePart = "(CurrentStatus == @CurrentStatus)";
                        if(status == "all")
                        {
                            currentStatusWherePart = string.Empty;
                        }

                        switch(assignedTo)
                        {
                            case "all":
                                StatusTicketsLinqDataSource.Where = string.Format("{0}", currentStatusWherePart);
                                break;
                            case "unassigned":
                                StatusTicketsLinqDataSource.Where = string.Format("{0} {1} (AssignedTo == null)", currentStatusWherePart, ((status != "all")? "&&": string.Empty));
                                break;
                            default:
                                StatusTicketsLinqDataSource.Where = string.Format("{1} {2} (AssignedTo == \"{0}\")", assignedTo, currentStatusWherePart, ((status != "all") ? "&&" : string.Empty));
                                break;
                        }

                        switch(status)
                        {
                            case "moreinfo":
                                StatusTicketsLinqDataSource.WhereParameters["CurrentStatus"].DefaultValue = "More Info";
                                break;
                            case "active":
                                StatusTicketsLinqDataSource.WhereParameters["CurrentStatus"].DefaultValue = "Active";
                                break;
                            case "resolved":
                                StatusTicketsLinqDataSource.WhereParameters["CurrentStatus"].DefaultValue = "Resolved";
                                break;
                            case "closed":
                                StatusTicketsLinqDataSource.WhereParameters["CurrentStatus"].DefaultValue = "Closed";
                                break;
                            default:
                                break;
                        }

                        TicketListControl.DataSourceID = "StatusTicketsLinqDataSource";
                    }
                    break;
                case "tagsandcategories":
                    TagListCell.Visible = true;
                    if(!string.IsNullOrEmpty(tagName))
                    {
                        TicketListControl.DataSourceID = "TagListTicketsLinqDataSource";
                    }
                    else if(!string.IsNullOrEmpty(category))
                    {
                        TicketListControl.DataSourceID = "CategoryTicketsLinqDataSource";
                    }
                    else
                    {
                        bindList = false;
                        TicketListControl.Visible = false;
                    }

                    break;

                default:
                    //do nothing
                    break;
            }
            if(bindList)
            {
                TicketListControl.DataBind();
            }
           
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
          
           
        }

       

        private void BuildStatusList()
        {
            //StatusList.SelectedIndex = -1;
            StatusPanel.Visible = true;
            if(!string.IsNullOrEmpty(status))
            {
                StatusList.Items.FindByValue(status).Selected = true;
            }
        }
        private void BuildStaffList(bool includeAll, bool includeUnassigned)
        {
            StaffUserList.SelectedIndex = -1;
            StaffUserPanel.Visible = true;
            StaffUserList.Items.Clear();
            if(includeAll)
            {
                StaffUserList.Items.Add(new ListItem("-- All Users --", "all"));
            }
            if(includeUnassigned)
            {
                StaffUserList.Items.Add(new ListItem("-- Unassigned --", "unassigned"));
            }
            StaffUserList.Items.AddRange(GetStaffUserList());
            if(!string.IsNullOrEmpty(assignedTo))
            {
                StaffUserList.Items.FindByValue(assignedTo).Selected = true;
            }
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


        public bool IsSelectedCategory(string cat)
        {

            return (!string.IsNullOrEmpty(category) && category.ToUpperInvariant() == cat.ToUpperInvariant());
        }

        public bool IsSelectedTag(string tag)
        {

            return (!string.IsNullOrEmpty(tagName) && tagName.ToUpperInvariant() == tag.ToUpperInvariant());
        }

        public string GetUrlForCategory(string cat)
        {
            return string.Format("TicketCenter.aspx?View=tagsandcategories&Category={0}", cat);
        }

        public string GetUrlForTag(string tag)
        {
            return string.Format("TicketCenter.aspx?View=tagsandcategories&TagName={0}", tag);
        }

        protected void StaffDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string statusUrlPart = GetStatusUrlPart();
           
            Page.Response.Redirect(string.Format("TicketCenter.aspx?View={0}{2}&User={1}", viewMode, StaffUserList.SelectedValue, statusUrlPart), true);
        }

        protected void StatusDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string staffUrlPart = GetStaffUrlPart();

            Page.Response.Redirect(string.Format("TicketCenter.aspx?View={0}&Status={1}{2}", viewMode, StatusList.SelectedValue, staffUrlPart), true);
        }

        private string GetStaffUrlPart()
        {
            string staffUrlPart = string.Empty;
            if(assignedTo != null)
            {
                staffUrlPart = string.Format("&User={0}", assignedTo);
            }
            return staffUrlPart;
        }
        private string GetStatusUrlPart()
        {
            string statusUrlPart = string.Empty;
            if(StatusList.Visible && status != null)
            {
                statusUrlPart = string.Format("&Status={0}", status);
            }
            return statusUrlPart;
        }



    }
}
