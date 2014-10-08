using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using TicketDesk.Domain;
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureDatabase()
        {   //kill initializer - features that may need one will reset this later
            Database.SetInitializer<TicketDeskContext>(null);
            //set the std migrator for identity
            Database.SetInitializer<TicketDeskIdentityContext>(new TicketDeskIdentityDbInitializer());
            DatabaseConfig.RegisterDatabase();

        }
    }
}