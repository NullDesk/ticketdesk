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

namespace TicketDesk.Controls
{
    public partial class ListViewListManager : System.Web.UI.UserControl
    {
        private ListViewSettingsCollection userSettings = ListViewSettingsCollection.GetSettingsForUser();
        //private ListViewSettings listSettings;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ListViewRepeater.DataSource = userSettings.Settings.Where(us => us.ListViewName != "search").OrderBy(us => us.ListViewDisplayOrder);
                DataBind();
            }
        }

        protected bool IsCurrentList(string listNameToCheck)
        {
            string listName = Page.Request.QueryString["list"];
            if (string.IsNullOrEmpty(listName))
            {
                int minOrder = userSettings.Settings.Min(us => us.ListViewDisplayOrder);
                listName = userSettings.Settings.SingleOrDefault(us => us.ListViewDisplayOrder == minOrder).ListViewName;
            }
            return (listName == listNameToCheck);
        }
    }
}