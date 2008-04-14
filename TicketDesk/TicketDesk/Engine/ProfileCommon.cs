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
        /// Gets or sets the ticket list sort field.
        /// </summary>
        /// <remarks>
        /// The default is the LastUpdateDate if not specified in the user's profile nor web.config
        /// </remarks>
        /// <value>The ticket list sort field.</value>
        public string TicketListSortExpressions
        {
            get
            {
                return (string)HttpContext.Current.Profile.GetPropertyValue("TicketListSortExpressions");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("TicketListSortExpressions", value);
            }
        }

      

        /// <summary>
        /// Gets or sets the ticket list items per page.
        /// </summary>
        /// <remarks>
        /// The default is 20 if not specified in the user's profile nor web.config
        /// </remarks>
        /// <value>The ticket list items per page.</value>
        public int TicketListItemsPerPage
        {
            get
            {
                if(HttpContext.Current.Profile.GetPropertyValue("TicketListItemsPerPage") == null)
                {
                    return 20;
                }
                return (int)HttpContext.Current.Profile.GetPropertyValue("TicketListItemsPerPage");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("TicketListItemsPerPage", value);
            }
        }
    }
}
