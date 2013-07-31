using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using TicketDesk.Legacy.Migrations;
using TicketDesk.Legacy;

namespace TicketDesk.Domain.Tests
{
    [TestClass]
    public class TicketDeskDatabaseInitializerTests
    {


        [ClassInitialize]
        public static void InitTests(TestContext context)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
            CleanUp();//just in case DBs exists from previous runs
        }

        private static void CleanUp()
        {
            using (var ctx = new TicketDeskContext("TicketDesk"))
            {
                ctx.Database.Delete();
            }
            using (var ctx = new EmptyDatabaseContext("TicketDesk2xRefernece"))
            {
                ctx.Database.Delete();
            }
        }
        
        [TestCleanup]
        public void CleanupTests()
        {
            CleanUp();
        }

        [TestMethod]
        public void CanInitializeNewDb()
        {

            using (var ctx = new TicketDeskContext("TicketDesk"))
            {
                var dbInit = new TicketDeskDatabaseInitializer("TicketDesk");
                dbInit.InitializeDatabase(ctx);

                var versionRow = ctx.Settings.SingleOrDefault(s => s.SettingName == "Version");
                var isTd2 = (versionRow != null && versionRow.SettingValue == "3.0.0.001");
                Assert.IsTrue(isTd2, "unable to verify TicketDesk 3.x DB created");
            }
        }

        [TestMethod]
        public void CanUpgradeLegacyDb()
        {

            MakeLegacyDb();

            using (var ctx = new TicketDeskContext("TicketDesk2xRefernece"))
            {
                var versionRow = ctx.Settings.SingleOrDefault(s => s.SettingName == "Version");
                var isTd2 = (versionRow != null && versionRow.SettingValue == "2.0.2");

                Assert.IsTrue(isTd2, "unable to verify reference TicketDesk 2.x DB created");

                MigrateLegacyDb();

                ctx.Entry(versionRow).Reload();
                var isTd3 = (versionRow != null && versionRow.SettingValue == "3.0.0.001");

                Assert.IsTrue(isTd3, "unable to verify reference TicketDesk 2.x DB migrated to 3.0.0.001");
            }


        }

        [TestMethod]
        public void CanDowngradeLegacyDb()
        {
            MakeLegacyDb();
            MigrateLegacyDb();
            UnMigrateLegacyDb();

            using (var ctx = new TicketDeskContext("TicketDesk2xRefernece"))
            {
                var versionRow = ctx.Settings.SingleOrDefault(s => s.SettingName == "Version");
                
                var isTd2 = (versionRow != null && versionRow.SettingValue == "2.0.2");

                Assert.IsTrue(isTd2, "unable to verify TicketDesk 2.x DB was downgraded");

            }
        }


        private void UnMigrateLegacyDb()
        {
            var upgradeConfig = new TicketDesk.Legacy.Migrations.Configuration();
            upgradeConfig.TargetDatabase = new DbConnectionInfo("TicketDesk2xRefernece");

            //this will do nothing but add the migration history table with the exact same migration ID as is used by the standard migrator.
            var migrator = new DbMigrator(upgradeConfig);
            migrator.Update("0");
        }

        private void MigrateLegacyDb()
        {
            var ctx = new TicketDeskLegacyContext("TicketDesk2xRefernece");
            var dbInit = new LegacyDatabaseInitializer("TicketDesk2xRefernece");
            dbInit.InitializeDatabase(ctx);
        }

        private void MakeLegacyDb()
        {
            var reader = System.IO.File.OpenText(@"..\..\SqlScripts\TicketDesk2DB\TicketDeskObjects.sql");
            var sql = reader.ReadToEnd();

            Database.SetInitializer<EmptyDatabaseContext>(new DropCreateDatabaseAlways<EmptyDatabaseContext>());
            using (var ctx = new EmptyDatabaseContext("TicketDesk2xRefernece"))
            {
                ctx.Database.Initialize(true);
                ctx.Database.ExecuteSqlCommand(sql);
            }
        }

       
    }
}
