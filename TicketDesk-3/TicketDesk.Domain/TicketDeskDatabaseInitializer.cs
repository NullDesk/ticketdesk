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
        public static void InitDatabase(string nameOrConnectionString)
        {
            System.Data.Entity.Database.SetInitializer<TicketDeskContext>(new TicketDeskDatabaseInitializer(nameOrConnectionString));
            using (var ctx = new TicketDeskContext(nameOrConnectionString))
            {
                ctx.Database.Initialize(false);
            }
        }

        private string ConnectionName { get; set; }
        public TicketDeskDatabaseInitializer(string connectionName)
        {
            ConnectionName = connectionName;
        }

        public void InitializeDatabase(TicketDeskContext context)
        {

            //run the standard set of TD 3x migrations
            //      If this was an upgrade of a legacy ticketdesk database, the legacy migrator will have run a different initial migration 
            //      with the same id as the regular migrator's initial migration. This allows it to skip trying to re-create all the tables,
            //      but still allows the system to continue to update the DB normally for all futher migrations
            var config = new Configuration();
            config.TargetDatabase = new DbConnectionInfo(ConnectionName);

            var migrator = new DbMigrator(config);
            migrator.Update();
        }



    }


}
