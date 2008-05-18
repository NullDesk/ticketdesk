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
using System.Collections.Generic;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.ComponentModel;
using TicketDesk.Engine.ListView;

namespace TicketDesk
{
    public partial class TicketCenter2 : System.Web.UI.Page
    {
        private ListViewSettingsCollection userSettings;
        private ListViewSettings listSetting;

        protected void Page_Load(object sender, EventArgs e)
        {
            userSettings =  ListViewSettingsCollection.GetSettingsForUser();
            listSetting = userSettings.GetSettingsForList("Unassigned");
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            
            TicketDataDataContext ctx = new TicketDataDataContext();
            var q = from t in ctx.Tickets

                    select t;

            var qfs = q.ApplyListViewSettings(listSetting, 0);

            TicketListView.DataSource = qfs;

            TicketListView.DataBind();
        }

        protected void SortLinkPreRendering(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            ListViewSortColumn sortCol = listSetting.SortColumns.SingleOrDefault(sc => sc.ColumnName == btn.CommandArgument);
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

        protected void TicketListDataPager_Load(object sender, EventArgs e)
        {
        }

        protected void ListViewSettingsEditorControl_SettingsChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void TicketListView_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName == "ChangeSort")
            {
                ListViewSortColumn sortCol = listSetting.SortColumns.SingleOrDefault(sc => sc.ColumnName == e.CommandArgument.ToString());
                if (sortCol != null)// column already in sort, just flip direction
                {
                    sortCol.SortDirection = (sortCol.SortDirection == ColumnSortDirection.Ascending)? ColumnSortDirection.Descending : ColumnSortDirection.Ascending;
                }
                else // column not in sort, replace sort with new simple sort for column
                {
                    listSetting.SortColumns.Clear();
                    listSetting.SortColumns.Add(new ListViewSortColumn(e.CommandArgument.ToString(), ColumnSortDirection.Ascending));
                }
                userSettings.Save();
                
                Bind();
                ListViewSettingsEditorControl.BindAdvancedSortOrderList();
            }
        }
    }
}
