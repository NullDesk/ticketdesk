using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Repositories
{
    public interface IDatabaseSchemaManagerRepository
    {
        void EnsureSchemaVersion();
    }
}
