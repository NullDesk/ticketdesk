// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Data.Entity;
using System.Data.Entity.Migrations;
using TicketDesk.Domain.Legacy.Migrations;

namespace TicketDesk.Domain.Legacy
{
    public class TdLegacyDatabaseInitializer<T> : IDatabaseInitializer<T> where T : DbContext
    {
        public static void InitDatabase(T context)
        {
            Database.SetInitializer(new TdLegacyDatabaseInitializer<T>());
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
