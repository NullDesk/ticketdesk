using System.Data.Entity;
using System.Data.Entity.Migrations;
using TicketDesk.Domain.Legacy.Migrations;

namespace TicketDesk.Domain.Legacy
{
    public class TicketDeskLegacyDatabaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public static void InitDatabase(T context)
        {
            Database.SetInitializer(new TicketDeskLegacyDatabaseInitializer<T>());
            using (var legacyCtx = context)
            {
                legacyCtx.Database.Initialize(true);
            }
        }


        public void InitializeDatabase(T context)
        {
            var upgradeConfig = new Configuration<T>();
            //this add the migration history table with the exact same migration ID as used by the standard migrator.
            var migrator = new DbMigrator(upgradeConfig);
            migrator.Update("InitialCreate");

        }
    }
}
