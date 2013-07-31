using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TicketDesk.Legacy
{
    public class TicketDeskLegacyContext : DbContext
    {

        public TicketDeskLegacyContext(string nameOrConnectionString) 
            : base(nameOrConnectionString) { }

        public TicketDeskLegacyContext()
            : base("TicketDesk")
        { }

    }
}
