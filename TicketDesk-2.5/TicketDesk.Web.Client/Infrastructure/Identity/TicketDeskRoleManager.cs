using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketDesk.Web.Client
{
    public class TicketDeskRoleManager : RoleManager<IdentityRole>
    {
        public TicketDeskRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        

        ////TODO: Why are options passed here, and what are they for? This is from the Microsoft.AspNet.Identity.Samples package
        //public static TicketDeskRoleManager Create(IdentityFactoryOptions<TicketDeskRoleManager> options, IOwinContext context)
        //{
        //    return new TicketDeskRoleManager(new RoleStore<IdentityRole>(context.Get<TicketDeskIdentityContext>()));
        //}

    }
}