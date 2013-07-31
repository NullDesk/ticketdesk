using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Tests
{
    public class EmptyDatabaseContext : DbContext
    {
            public EmptyDatabaseContext(string nameOrConnectionString)
                : base(nameOrConnectionString) { }

            
    }
}
