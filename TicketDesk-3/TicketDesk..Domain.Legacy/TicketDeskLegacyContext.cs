using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TicketDesk.Domain.Legacy
{
    public class TicketDeskLegacyContext : DbContext
    {

        public TicketDeskLegacyContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        public TicketDeskLegacyContext() : this("TicketDesk") { }

    }
}
