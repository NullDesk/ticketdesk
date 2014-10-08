using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Web.Identity.Migrations;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity
{
    public class TicketDeskIdentityDbInitializer : MigrateDatabaseToLatestVersion<TicketDeskIdentityContext, Configuration>
    {
        //no implementation, defined here to simplify and unify naming conventions and usage patterns 
    }

}
