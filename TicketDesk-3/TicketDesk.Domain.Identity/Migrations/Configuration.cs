namespace TicketDesk.Domain.Identity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketDesk.Domain.Identity.TicketDeskIdentityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "TicketDeskIdentity";
        }

        protected override void Seed(TicketDesk.Domain.Identity.TicketDeskIdentityContext context)
        {
            
        }
    }
}
