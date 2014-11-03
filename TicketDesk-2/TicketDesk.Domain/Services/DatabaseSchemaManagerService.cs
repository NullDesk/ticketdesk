using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Repositories;

namespace TicketDesk.Domain.Services
{
    [Export(typeof(IDatabaseSchemaManagerService))]
    public class DatabaseSchemaManagerService : IDatabaseSchemaManagerService
    {
        [ImportingConstructor]
        public DatabaseSchemaManagerService(IDatabaseSchemaManagerRepository repository)
        {
            Repository = repository;
        }

        private IDatabaseSchemaManagerRepository Repository { get; set; }

        public void EnsureSchemaVersion()
        {
            Repository.EnsureSchemaVersion();
        }
    }
}
