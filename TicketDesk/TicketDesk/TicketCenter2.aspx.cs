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

        protected void Page_Load()
        {
            userSettings = ListViewSettingsCollection.GetSettingsForUser();

            string listName = Page.Request.QueryString["list"];
            if (string.IsNullOrEmpty(listName))
            {
                int minOrder = userSettings.Settings.Min(us => us.ListViewDisplayOrder);
                listSetting = userSettings.Settings.SingleOrDefault(us => us.ListViewDisplayOrder == minOrder);
                listName = listSetting.ListViewName;
            }
            Page.Title = string.Format("Ticket Center: {0}", listSetting.ListViewDisplayName);
            ListViewSettingsEditorControl.ListName = listName;
            ListViewControl.ListName = listName;

        }

        private void Bind()
        {
            ListViewControl.Bind();
        }

        protected void ListViewSettingsEditorControl_SettingsChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void ListViewControl_SettingsChanged(object sender, EventArgs e)
        {
            ListViewSettingsEditorControl.BindAdvancedSortOrderList();
        }
    }
}
