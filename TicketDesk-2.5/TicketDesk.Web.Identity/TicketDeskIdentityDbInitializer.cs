using System.Data.Entity;
using TicketDesk.Web.Identity.Migrations;

namespace TicketDesk.Web.Identity
{
    public class TicketDeskIdentityDbInitializer : MigrateDatabaseToLatestVersion<TicketDeskIdentityContext, Configuration>
    {
        //no implementation, defined here to simplify and unify naming conventions and usage patterns 
    }

}
