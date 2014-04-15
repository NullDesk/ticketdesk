using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Legacy;

namespace TicketDesk.Domain.Legacy.Migrations
{

    

    public sealed class Configuration : DbMigrationsConfiguration<TicketDeskLegacyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskDomain";

        }

        protected override void Seed(TicketDeskLegacyContext context)
        {

            
        }
    }
}
