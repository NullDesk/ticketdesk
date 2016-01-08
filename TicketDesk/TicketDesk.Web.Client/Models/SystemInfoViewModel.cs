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

using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using TicketDesk.Domain;
using TicketDesk.IO;
using TicketDesk.Search.Azure;

namespace TicketDesk.Web.Client.Models
{
    public class SystemInfoViewModel
    {
        private DatabaseInfoViewModel _databaseInfo;
        private DatabaseStatusViewModel _databaseStatus;
        private AzureInfoViewModel _azureInfo;

        public DatabaseInfoViewModel DatabaseInfo
        {
            get { return _databaseInfo ?? (_databaseInfo = new DatabaseInfoViewModel()); }
        }

        public DatabaseStatusViewModel DatabaseStatus
        {
            get { return _databaseStatus ?? (_databaseStatus = new DatabaseStatusViewModel()); }
        }

        public AzureInfoViewModel AzureInfo
        {
            get { return _azureInfo ?? (_azureInfo = new AzureInfoViewModel(DatabaseInfo.DbEdition, DatabaseInfo.DatabaseName)); }
        }


        public class DatabaseInfoViewModel
        {
            public DatabaseInfoViewModel()
            {
                var connectionString = ConfigurationManager.ConnectionStrings["TicketDesk"].ConnectionString;
                DbEdition = GetSqlEdition(connectionString);
                IsLocalDb = GetIsLocalDb(connectionString);
               
                var builder = new SqlConnectionStringBuilder(connectionString);
                var dsource = builder.DataSource.Split('\\');
                ServerName = dsource.Length > 0 ? dsource[0] : null;
                SqlInstance = dsource.Length > 1 ? dsource[1] : null;

                IsFileDatabase = !string.IsNullOrEmpty(builder.AttachDBFilename);
                DatabaseName = IsFileDatabase ? builder.AttachDBFilename : builder.InitialCatalog;


            }

            public string DbEdition { get; private set; }
            public bool IsLocalDb { get; private set; }
            public bool IsFileDatabase { get; private set; }
            public string ServerName { get; private set; }
            public string SqlInstance { get; private set; }
            public string DatabaseName { get; private set; }

            private string GetSqlEdition(string connectionString)
            {
                string edition = null;
                try
                {
                    var conn = GetMasterCatalogConnection(connectionString);
                    var cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 5;
                    cmd.CommandText = "SELECT SERVERPROPERTY('Edition')";
                    conn.Open();
                    edition = cmd.ExecuteScalar() as string;
                    conn.Close();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch { }
                return edition;
            }

            private bool GetIsLocalDb(string connectionString)
            {
                bool isLocalDb = false;
                try
                {
                    var conn = GetMasterCatalogConnection(connectionString);
                    var cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 5;
                    cmd.CommandText = "SELECT SERVERPROPERTY('IsLocalDB')";
                    conn.Open();
                    var ldb = cmd.ExecuteScalar() as int?;
                    isLocalDb = ldb.HasValue;
                    conn.Close();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch {  }
                return isLocalDb;
            }
            private SqlConnection GetMasterCatalogConnection(string connectionString)
            {

                var builder = new SqlConnectionStringBuilder(connectionString);
                builder.AttachDBFilename = "";
                builder.InitialCatalog = "Master";
                builder.ConnectTimeout = 5;
                return new SqlConnection(builder.ToString());
            }
        }

        public class DatabaseStatusViewModel
        {
            public DatabaseStatusViewModel()
            {
                using (var ctx = new TdDomainContext(null))
                {
                    DatabaseExists = ctx.Database.Exists();
                    IsCompatibleWithEfModel = DatabaseExists && !IsEmptyDatabase && ctx.Database.CompatibleWithModel(false);
                }
            }

            public bool DatabaseExists { get; private set; }
            public bool IsDatabaseReady { get { return DatabaseConfig.IsDatabaseReady; }}
            public bool IsLegacyDatabase { get { return DatabaseConfig.IsLegacyDatabase(); }}
            public bool IsEmptyDatabase { get { return DatabaseConfig.IsEmptyDatabase(); } }
            public bool IsCompatibleWithEfModel { get; private set; }
            public bool HasLegacySecurityObjects { get { return DatabaseConfig.HasLegacySecurity(); } }
        }

        public class AzureInfoViewModel
        {
            public AzureInfoViewModel(string dbEdition, string dbName)
            {
                AzureWebSiteName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
                IsAzureWebSite = !string.IsNullOrEmpty(AzureWebSiteName);
                IsSqlAzure = dbEdition != null && dbEdition.IndexOf("azure", StringComparison.InvariantCultureIgnoreCase) >= 0;
                AzureSqlService = IsSqlAzure ? dbName : null;
                AzureSearchService = GetAzureSearchServiceName();
                AzureStorageService = GetAzureStorageServiceName();


            }
            public string AzureWebSiteName { get; private set; }
            public bool IsAzureWebSite { get; private set; }
            public string AzureSqlService { get; private set; }
            public bool IsSqlAzure { get; private set; }

            public string AzureSearchService { get; private set; }
            public string AzureStorageService { get; private set; }


            public bool HasAzureServices
            {
                get
                {
                    return
                        IsAzureWebSite ||
                        IsSqlAzure ||
                        !string.IsNullOrEmpty(AzureSearchService) ||
                        !string.IsNullOrEmpty(AzureStorageService);

                }
            }

            private string GetAzureSearchServiceName()
            {
                string svc = null;
                var azSearchInfo = AzureSearchConector.TryGetInfoFromConnectionString() ?? AzureSearchConector.TryGetInfoFromAppSettings();
                if (azSearchInfo != null)
                {
                    svc = azSearchInfo.ServiceName;
                }
                return svc;
            }
            private string GetAzureStorageServiceName()
            {
                string svcName = null;
                var azStore = !string.IsNullOrEmpty(AzureConnectionHelper.ConfigManagerConnString)
                    ? AzureConnectionHelper.ConfigManagerConnString
                    : AzureConnectionHelper.CloudConfigConnString;

                if (!string.IsNullOrEmpty(azStore))
                {
                    try
                    {
                        var parts = azStore
                            .Split(';')
                            .Select(p => p.Split('='))
                            .Select(t => new { key = t[0], value = t[1] }).ToList();

                        var service =
                            parts.FirstOrDefault(
                                p => p.key.Equals("AccountName", StringComparison.InvariantCultureIgnoreCase));
                        if (service != null)
                        {
                            svcName = service.value;

                        }
                    }
                    // ReSharper disable once EmptyGeneralCatchClause
                    catch
                    {
                    }

                }
                return svcName;
            }
        }
    }
}