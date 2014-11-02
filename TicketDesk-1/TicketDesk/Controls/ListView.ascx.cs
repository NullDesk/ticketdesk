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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine.ListView;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class ListView : System.Web.UI.UserControl
    {



        
        private string _where;
        private ListViewSettingsCollection userSettings = ListViewSettingsCollection.GetSettingsForUser();
        private ListViewSettings _listSettings;

        private ListViewSettings ListSettings
        {
            get 
            {
                if (_listSettings == null && !string.IsNullOrEmpty(ListName))
                {
                    _listSettings = userSettings.GetSettingsForList(ListName);
                }
                return _listSettings; 
            }
            set { _listSettings = value; }
        }


        public event EventHandler SettingsChanged;

        public string Where
        {
            get { return _where; }
            set
            {
                _where = value;

            }
        }

        public string ListName
        {
            get 
            {
                return ViewState["TicketCenterListName"] as string;
            }
            set { ViewState["TicketCenterListName"] = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ListName))
            {
                throw new ApplicationException("List Name was not supplied to ListView user control");
            }
            

            TicketsLinqDataSource.Where = Where;

        }

        public void Bind()
        {
            SetListPageSize();
            if (Page.IsPostBack)
            {
                TicketListView.DataBind();
            }
        }

        public void OnSettingsChanged()
        {

            EventHandler evt = SettingsChanged;
            if (evt != null)
            {
                evt(this, EventArgs.Empty);
            }
        }

        protected void TicketListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeSort")
            {
                ListViewSortColumn sortCol = ListSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == e.CommandArgument.ToString());
                if (sortCol != null)// column already in sort, just flip direction
                {
                    sortCol.SortDirection = (sortCol.SortDirection == ColumnSortDirection.Ascending) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    ListSettings.SortColumns.Clear();
                    ListSettings.SortColumns.Add(new ListViewSortColumn(e.CommandArgument.ToString(), ColumnSortDirection.Ascending));
                }
                userSettings.Save();

                Bind();
                OnSettingsChanged();
            }
        }
        protected void TicketListView_LayoutCreated(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)//set this only on first load... seems to cause infinite an loop if used on postbacks where the postback may have rebound the list.
            {
                SetListPageSize();
            }
        }

        private void SetListPageSize()
        {
            DataPager pager = TicketListView.FindControl("TicketListDataPager") as DataPager;
            if (pager != null)
            {
                pager.PageSize = ListSettings.ItemsPerPage;
            }
        }
        

        protected void SortLinkPreRendering(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            ListViewSortColumn sortCol = ListSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == btn.CommandArgument);
            if (sortCol != null)
            {
                string imgUrl = string.Format("~/Controls/Images/{0}.png", (sortCol.SortDirection == ColumnSortDirection.Ascending) ? "up" : "down");
                btn.ImageUrl = imgUrl;

            }
            else
            {
                btn.ImageUrl = "~/Controls/Images/nobutton.gif";
            }
        }

        protected void TicketsLinqDataSource_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {

            TicketDataDataContext ctx = new TicketDataDataContext();
            var q = from t in ctx.Tickets
                    select t;

            var qfs = q.ApplyListViewSettings(ListSettings, 0);


            e.Result = qfs;

        }

    }
}