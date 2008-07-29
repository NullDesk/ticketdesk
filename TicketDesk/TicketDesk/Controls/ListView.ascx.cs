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
        private string _listName;
        private string _where;
        private ListViewSettingsCollection userSettings = ListViewSettingsCollection.GetSettingsForUser();
        private ListViewSettings listSettings;

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
            get { return _listName; }
            set { _listName = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ListName))
            {
                throw new ApplicationException("List Name was not supplied to ListView user control");
            }
            listSettings = userSettings.GetSettingsForList(ListName);
            TicketsLinqDataSource.Where = Where;
            
        }

        public void Bind()
        {
            TicketListView.DataBind();
           
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
                ListViewSortColumn sortCol = listSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == e.CommandArgument.ToString());
                if (sortCol != null)// column already in sort, just flip direction
                {
                    sortCol.SortDirection = (sortCol.SortDirection == ColumnSortDirection.Ascending) ? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    listSettings.SortColumns.Clear();
                    listSettings.SortColumns.Add(new ListViewSortColumn(e.CommandArgument.ToString(), ColumnSortDirection.Ascending));
                }
                userSettings.Save();

                Bind();
                OnSettingsChanged();
            }
        }

        protected void TicketListDataPager_Load(object sender, EventArgs e)
        {
        }

        protected void SortLinkPreRendering(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            ListViewSortColumn sortCol = listSettings.SortColumns.SingleOrDefault(sc => sc.ColumnName == btn.CommandArgument);
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

            var qfs = q.ApplyListViewSettings(listSettings, 0);


            e.Result = qfs;
            
        }

    }
}