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