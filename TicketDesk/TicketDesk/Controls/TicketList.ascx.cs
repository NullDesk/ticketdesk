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
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine;

namespace TicketDesk.Controls
{
    public partial class TicketList : System.Web.UI.UserControl
    {
        private ProfileCommon pc = new ProfileCommon();
        public string DataSourceID
        {
            get
            {
                return TicketListView.DataSourceID;
            }
            set
            {
                TicketListView.DataSourceID = value;
            }
        }

        protected void TicketListView_Sorted(object sender, EventArgs e)
        {
            
            ProfileCommon pc = new ProfileCommon();
            LinkButton oldSortLink = (LinkButton)TicketListView.FindControl(pc.TicketListSortField + "SortLink");
            
            pc.TicketListSortField = TicketListView.SortExpression;
            pc.TicketListSortDirection = TicketListView.SortDirection;
            LinkButton newSortLink = (LinkButton)TicketListView.FindControl(TicketListView.SortExpression + "SortLink");
            if(oldSortLink != null)
            {
                SortLinkLoading(oldSortLink, EventArgs.Empty);
            }
            if(newSortLink != null)
            {
                SortLinkLoading(newSortLink, EventArgs.Empty);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            
            if(!Page.IsPostBack && this.Visible)
            {
                ShowList();
            }
        }

        public void ShowList()
        {
            if(string.IsNullOrEmpty(pc.TicketListSortField))
            {
                pc.TicketListSortField = "LastUpdateDate";
                pc.TicketListSortDirection = SortDirection.Descending;

            }
            DataPager dp = (DataPager)TicketListView.FindControl("TicketListDataPager");
            if(dp != null && dp.PageSize != pc.TicketListItemsPerPage)
            {
                dp.PageSize = pc.TicketListItemsPerPage;
            }
            TicketListView.Sort(pc.TicketListSortField, pc.TicketListSortDirection);
        }

       

        protected void SortLinkLoading(object sender, EventArgs e)
        {
            string sortTextSuffix = string.Empty;
            LinkButton btn = (LinkButton)sender;
            if(btn.CommandArgument == pc.TicketListSortField)
            {

                if(pc.TicketListSortDirection == SortDirection.Ascending)
                {
                    sortTextSuffix = "&nbsp;˄";
                }
                else
                {
                    sortTextSuffix = "&nbsp;˅";
                }
            }
            Label sortDirection = (Label)btn.NamingContainer.FindControl("SortDirection" + btn.CommandArgument);

            sortDirection.Text = string.Format("{0}",  sortTextSuffix);
        }

        protected void PageSizeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dl = (DropDownList)sender;
            pc.TicketListItemsPerPage = Convert.ToInt32(dl.SelectedValue);
            DataPager dp = (DataPager)TicketListView.FindControl("TicketListDataPager");
            if(dp != null && dp.PageSize != pc.TicketListItemsPerPage)
            {
                dp.PageSize = pc.TicketListItemsPerPage;
            }
            
        }

        protected void PageSizeDropDownList_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                DropDownList dl = (DropDownList)sender;
                int item = dl.Items.IndexOf(dl.Items.FindByValue(pc.TicketListItemsPerPage.ToString()));
                dl.SelectedIndex = item;
            }
        }

        protected void TicketListDataPager_Load(object sender, EventArgs e)
        {
            DataPager dp = (DataPager)sender;
            if(dp != null && dp.PageSize != pc.TicketListItemsPerPage)
            {
                dp.PageSize = pc.TicketListItemsPerPage;
            }
        }

    }
}