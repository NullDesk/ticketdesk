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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AjaxControlToolkit;
using TicketDesk.Engine.Linq;
using TicketDesk.Engine;
using System.Web.UI.HtmlControls;

namespace TicketDesk.Controls
{
    public partial class TicketList : System.Web.UI.UserControl
    {
        #region general

        public event EventHandler ColumnSortChanged;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            InitSorts();

            if(!Page.IsPostBack && this.Visible)
            {
                ShowList();
            }
        }

        private void InitSorts()
        {
            sortExpressions = new UserSortExpressions();
            sort = sortExpressions.GetSortExpression(DataSourceID);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if(TicketListView.Items.Count > 0)
            {

                foreach(LinkButton lb in GetAllSortButtons())
                {
                    SortLinkLoading(lb, EventArgs.Empty);
                }
            }
        }

        public string GetFriendlyColumnNameForDataField(string dataFieldName)
        {
            string[] expArr = dataFieldName.Split(' ');
            for(int i = 0; i < expArr.Length; i++)
            {
                if(expArr[i] != "ASC" && expArr[i] != "DESC")
                {
                    LinkButton btn = (LinkButton)TicketListView.FindControl(expArr[i] + "SortLink");
                    if(btn != null)
                    {
                        expArr[i] = btn.Text;
                    }
                }
            }
            return string.Join(" ", expArr);
        }

        public List<LinkButton> GetAllSortButtons()
        {
            List<LinkButton> btns = new List<LinkButton>();
            HtmlTable table = (HtmlTable)TicketListView.FindControl("itemPlaceholderContainer");
            if(table != null)
            {
                foreach(Control ctrl in table.GetControls(true))
                {
                    LinkButton lb = ctrl as LinkButton;
                    if(lb != null && lb.CommandName == "ChangeSort")
                    {
                        btns.Add(lb);
                    }
                }
            }
            return btns;
        }

        public List<string> GetAllSortableDataFields()
        {
            var cols = from lb in GetAllSortButtons()
                       select lb.CommandArgument;
            return cols.ToList();
        }


        public void ReSortList()
        {
            InitSorts();
            ShowList();
        }
        public void ShowList()
        {
            if(sort == null)
            {
                InitSorts();
            }
            DataPager dp = (DataPager)TicketListView.FindControl("TicketListDataPager");
            if(dp != null && dp.PageSize != pc.TicketListItemsPerPage)
            {
                dp.PageSize = pc.TicketListItemsPerPage;
            }
            TicketListView.Sort(sort.TruncatedSortExpression, sort.Direction);

        }

        #endregion

        #region listview paging

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

        #endregion

        #region listview sorting

        UserSortExpressions sortExpressions;
        UserSort sort;
        private ProfileCommon pc = new ProfileCommon();

        protected void SortLinkLoading(object sender, EventArgs e)
        {
            string sortTextSuffix = string.Empty;
            LinkButton btn = (LinkButton)sender;
            SortDirection? sDir = sort.GetSortDirectionForColumn(btn.CommandArgument);
            if(sDir.HasValue)
            {
                sortTextSuffix = (sDir.Value == SortDirection.Ascending) ? "&nbsp;˄" : "&nbsp;˅";
            }
            Label sortDirection = (Label)btn.NamingContainer.FindControl("SortDirection" + btn.CommandArgument);

            sortDirection.Text = sortTextSuffix;
        }

        #endregion

        protected void TicketListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if(e.CommandName == "ChangeSort")
            {
                string[] sortExpr = sort.GetSortColumnExpressions();
                List<string> newSortExpressions = new List<string>();
                if(sort.GetSortColumns().Contains(e.CommandArgument as string))// column already in sort, just flip direction
                {

                   
                    foreach(string sortExp in sortExpr)
                    {
                        if(sortExp.StartsWith(e.CommandArgument as string))
                        {
                            string truncSort = UserSortExpressions.GetTruncatedSortExpression(sortExp);
                            SortDirection oldDir = UserSortExpressions.GetDirectionFromSortExpression(sortExp);
                            SortDirection newDir = (oldDir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
                            newSortExpressions.Add(string.Format("{0} {1}", truncSort, (newDir == SortDirection.Ascending) ? "ASC" : "DESC"));
                        }
                        else
                        {
                            newSortExpressions.Add(sortExp);
                        }

                    }
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    newSortExpressions.Add(e.CommandArgument as string + " ASC"); 
                }
                string newExpression = string.Join(",", newSortExpressions.ToArray());
                sortExpressions.SetSortExpression(DataSourceID, UserSortExpressions.GetTruncatedSortExpression(newExpression), UserSortExpressions.GetDirectionFromSortExpression(newExpression));
                ReSortList();
                OnColumnSortChanged(sender);
            }
        }

        public void OnColumnSortChanged(object sender)
        {
            if(ColumnSortChanged != null)
            {
                ColumnSortChanged(sender, EventArgs.Empty);
            }
        }

    }
}