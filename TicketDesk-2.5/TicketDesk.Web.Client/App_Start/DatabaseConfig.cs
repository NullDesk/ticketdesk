using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using Configuration = TicketDesk.Domain.Migrations.Configuration;

namespace TicketDesk.Web.Client
{
    public static class DatabaseConfig
    {
        public static void RegisterDatabase()
        {
           
            var setupEnabled = ConfigurationManager.AppSettings["ticketdesk:SetupEnabled"];
            var firstRunEnabled = !string.IsNullOrEmpty(setupEnabled) && setupEnabled.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            
            if (firstRunEnabled && !DatabaseConfig.IsDatabaseReady)
            {
                //add a global filter to send requests to the database managment first run functions
                GlobalFilters.Filters.Add(new DbSetupFilter());
            }
            else
            {
                //database exists and isn't legacy or empty, but it could still need migration
                if (DatabaseConfig.HasPendingMigrations())
                {
                    //run any pending migrations automatically to bring the DB up to date
                    Database.SetInitializer<TicketDeskContext>(
                        new MigrateDatabaseToLatestVersion
                            <TicketDeskContext, TicketDesk.Domain.Migrations.Configuration>());
                    using (var ctx = new TicketDeskContext())
                    {
                        ctx.Database.Initialize(true);
                    }
                }
            }
        }

        public static bool IsDatabaseReady
        {
            get
            {
                var result = false;
                using (var ctx = new TicketDeskContext())
                {
                    result = (ctx.Database.Exists() && !IsEmptyDatabase(ctx)) && !IsLegacyDatabase(ctx);
                }
                return result;
            }
        }

        public static bool HasPendingMigrations()
        {
            var result = false;
            var mgt = new DbMigrator(new Configuration());
            return mgt.GetPendingMigrations().Any();
        }

        public static bool IsEmptyDatabase()
        {
            using (var ctx = new TicketDeskContext())
            {
                return IsEmptyDatabase(ctx);
            }
        }
        public static bool IsLegacyDatabase()
        {
            using (var ctx = new TicketDeskContext())
            {
                return IsLegacyDatabase(ctx);
            }
        }

        private static bool IsEmptyDatabase(TicketDeskContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            var hasTables = false;
            var numTables = context.Database.SqlQuery<int>(
                "SELECT count(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'");
            hasTables = numTables.First() > 0;

            return !hasTables;
        }

        private static bool IsLegacyDatabase(TicketDeskContext context)
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