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
    /// Entity object that encapsulates common user data
    /// </summary>
    /// <remarks>
    /// Instances of this class will contain both the user name and the 
    /// display name for the user. 
    /// </remarks>
    public class User
    {
        private string _name;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private string _displayName;

        /// <summary>
        /// Gets or sets the display name for the user.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="displayName">The display name of the user.</param>
        public User(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }
    }
}
