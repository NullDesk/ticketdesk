using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Models
{
    public class UserInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">The display name.</param>
        public UserInfo(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets or (private) sets the user name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or (private) sets the user's display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get;
            private set;
        }


    }
}
