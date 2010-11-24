using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Models
{
    public class UserInfo
    {
        /// <summary>
        /// Gets or (private) sets the user name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or (private) sets the user's display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get;
            set;
        }


    }
}
