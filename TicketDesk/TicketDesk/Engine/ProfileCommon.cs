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
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using TicketDesk.Engine.ListView;

namespace TicketDesk.Engine
{
    /// <summary>
    /// Contains profile fields for TicketDesk users.
    /// </summary>
    /// <remarks>
    /// Web application projects do not auto-generate this class, so 
    /// it has to be manually created in order to interact with the 
    /// Profile object in the same way as would be done with a web site
    /// project.
    /// </remarks>
    public class ProfileCommon
    {


        /// <summary>
        /// Gets or sets the user's ticket ListView settings.
        /// </summary>
        /// <value>The ticket list view settings.</value>
        public ListViewSettingsCollection TicketListSettings
        {
            get
            {
                 var lv = (ListViewSettingsCollection)HttpContext.Current.Profile.GetPropertyValue("TicketListSettings");
                 if (lv.Settings.Count < 1)
                 {
                     ListViewSettingsCollection.CreateNewSettings(lv);
                 }
                 return lv;
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("TicketListSettings", value);
            }
        }
    }
}
