using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Migrations;

namespace TicketDesk.Domain
{
    public class TicketDeskDatabaseInitializer : IDatabaseInitializer<TicketDeskContext>
    {
        public static void InitDatabase(string connectionString, string providerInvariantName)
        {
            System.Data.Entity.Database.SetInitializer<TicketDeskContext>(new TicketDeskDatabaseInitializer(connectionString, providerInvariantName));
            using (var ctx = new TicketDeskContext(connectionString))
            {
                ctx.Database.Initialize(false);
            }
        }

        private string ConnectionString { get; set; }
        public string ProviderInvariantName { get; set; }

        public TicketDeskDatabaseInitializer(string connectionString, string providerInvariantName)
        {
            ConnectionString = connectionString;
            ProviderInvariantName = providerInvariantName;
        }

        public void InitializeDatabase(TicketDeskContext context)
        {

            //run the standard set of TD 3x migrations
            //      If this was an upgrade of a legacy ticketdesk database, the legacy migrator will have run a different initial migration 
            //      with the same id as the regular migrator's initial migration. This allows it to skip trying to re-create all the tables,
            //      but still allows the system to continue to update the DB normally for all futher migrations
            var config = new Configuration();
            var targ = new DbConnectionInfo(ConnectionString, ProviderInvariantName);
           
            var migrator = new DbMigrator(config);
            migrator.Configuration.TargetDatabase = targ;
            migrator.Update();
        }



    }


}
