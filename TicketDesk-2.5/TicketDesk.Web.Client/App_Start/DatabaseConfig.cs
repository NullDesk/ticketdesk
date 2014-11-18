using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Infrastructure;
using TicketDesk.Web.Identity.Infrastructure;
using Configuration = TicketDesk.Domain.Migrations.Configuration;

namespace TicketDesk.Web.Client
{
    public static class DatabaseConfig
    {
        public static void RegisterDatabase()
        {

            var setupEnabled = ConfigurationManager.AppSettings["ticketdesk:SetupEnabled"];
            var firstRunEnabled = !string.IsNullOrEmpty(setupEnabled) && 
                setupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase);

            var demoRefresh = ConfigurationManager.AppSettings["ticketdesk:ResetDemoDataOnStartup"];
            var firstRunDemoRefresh = !string.IsNullOrEmpty(demoRefresh) && 
                demoRefresh.Equals("true", StringComparison.InvariantCultureIgnoreCase) && 
                IsDatabaseReady;//only do this if database was ready on startup, otherwise migrator will take care of it
            
            if (firstRunEnabled && !IsDatabaseReady)
            {
                //add a global filter to send requests to the database managment first run functions
                GlobalFilters.Filters.Add(new DbSetupFilter());
            }
            else
            {
                //run any pending migrations automatically to bring the DB up to date
                Database.SetInitializer<TicketDeskContext>(
                    new MigrateDatabaseToLatestVersion<TicketDeskContext, Configuration>(true));
                using (var ctx = new TicketDeskContext(null))
                {
                    ctx.Database.Initialize(false);
                    if (firstRunDemoRefresh)
                    {
                        DemoDataManager.SetupDemoData(ctx);
                    }
                }
            }
        }

        public static bool IsDatabaseReady
        {
            get
            {
                bool result;
                using (var ctx = new TicketDeskContext(null))
                {
                    result = (ctx.Database.Exists() && !IsEmptyDatabase(ctx)) && !IsLegacyDatabase(ctx);
                }
                return result;
            }
        }


        public static bool IsEmptyDatabase()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                return IsEmptyDatabase(ctx);
            }
        }

        public static bool IsLegacyDatabase()
        {
            using (var ctx = new TicketDeskContext(null))
            {
                return IsLegacyDatabase(ctx);
            }
        }

        private static bool IsEmptyDatabase(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var numTables = context.Database.SqlQuery<int>(
                "SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'");
            bool hasTables = numTables.First() > 0;

            return !hasTables;
        }

        private static bool IsLegacyDatabase(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var isLegacy = false;
            try
            {
                var oldVersion =
                    context.Database.SqlQuery<string>("select SettingValue from Settings where SettingName = 'Version'");
                isLegacy = (oldVersion != null && oldVersion.Any() && oldVersion.First().Equals("2.0.2"));
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //eat any exception, we'll assume that if the db exists, but we can't read the settings, then it is an just empty new db
            }
            return isLegacy;
        }
    }
}