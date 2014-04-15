using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain.Identity
{
    //new UserManager<UserProfile>(new UserStore<UserProfile>(new TicketDeskContext())));

    public class TicketDeskUserManager : UserManager<UserProfile>
    {
        public TicketDeskUserManager()
            : this(new TicketDeskContext()) { }

        public TicketDeskUserManager(DbContext ctx)
            : this(new UserStore<UserProfile>(ctx)) { }

        private TicketDeskUserManager(IUserStore<UserProfile> store)
            : base(store)
        {
            PasswordValidator = new MinimumLengthValidator(4);
        }
    }
}
