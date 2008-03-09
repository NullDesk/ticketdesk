using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

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
        public string TicketListSortField
        {
            get
            {
                if(HttpContext.Current.Profile.GetPropertyValue("TicketListSortField") == null)
                {
                    return "LastUpdateDate";
                }
                return (string)HttpContext.Current.Profile.GetPropertyValue("TicketListSortField");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("TicketListSortField", value);
            }
        }

        /// <summary>
        /// Gets or sets the ticket list sort direction.
        /// </summary>
        /// <remarks>
        /// The default is a descending sort if not specified in the user's profile nor web.config
        /// </remarks>
        /// <value>The ticket list sort direction.</value>
        public System.Web.UI.WebControls.SortDirection TicketListSortDirection
        {
            get
            {
                if(HttpContext.Current.Profile.GetPropertyValue("TicketListSortDirection") == null)
                {
                    return SortDirection.Descending;
                }
                return (System.Web.UI.WebControls.SortDirection)HttpContext.Current.Profile.GetPropertyValue("TicketListSortDirection");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("TicketListSortDirection", value);
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
