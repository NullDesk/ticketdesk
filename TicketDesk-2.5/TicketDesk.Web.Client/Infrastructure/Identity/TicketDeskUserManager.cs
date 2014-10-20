using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Client
{
    public class TicketDeskUserManager : UserManager<TicketDeskUser>
    {
        public TicketDeskUserManager(IUserStore<TicketDeskUser> store)
            : base(store)
        {

        }

        public UserDisplayInfoCache InfoCache { get { return new UserDisplayInfoCache(this); } }


    }
}