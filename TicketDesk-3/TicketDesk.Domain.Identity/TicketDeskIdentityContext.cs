using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TicketDesk.Domain.Identity
{
    public class TdUser : User
    {
      public string HomeTown { get; set; }
    }
    public class TicketDeskIdentityContext: IdentityDbContext<TdUser, UserClaim, UserSecret, UserLogin, Role, UserRole, Token, UserManagement>
    {
        public TicketDeskIdentityContext() : base("TicketDesk") { }
        public TicketDeskIdentityContext(string nameOrConnectionString) : base(nameOrConnectionString) { } 

        
    }
}