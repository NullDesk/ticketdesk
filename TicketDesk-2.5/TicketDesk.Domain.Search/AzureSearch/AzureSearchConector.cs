using System.Configuration;
using System.Linq;
using RedDog.Search;
using RedDog.Search.Http;

namespace TicketDesk.Domain.Search.AzureSearch
{
    public abstract class AzureSearchConector
    {
        private ApiConnection connection;
        private IndexManagementClient managementClient;
        private IndexQueryClient queryClient;

        internal IndexManagementClient ManagementClient
        {
            get { return managementClient ?? (managementClient = new IndexManagementClient(Connection)); }
        }

        internal IndexQueryClient QueryClient
        {
            get { return queryClient ?? (queryClient = new IndexQueryClient(Connection)); }
        }

        internal ApiConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    var svcInfo = TryGetInfoFromConnectionString() ??
                                  TryGetInfoFromAppSettings();

                    connection = ApiConnection.Create(svcInfo.ServiceName, svcInfo.QueryKey);
                }
                return connection;
            }
        }

        private ServiceInfo TryGetInfoFromAppSettings()
        {
            ServiceInfo info = null;
            var service = ConfigurationManager.AppSettings["Azure.Search.ServiceName"];
            var key = ConfigurationManager.AppSettings["Azure.Search.Admin.Key"];
            if (service != null && key != null)
            {
                info = new ServiceInfo { ServiceName = service, QueryKey = key };
            }
            return info;
        }

        private ServiceInfo TryGetInfoFromConnectionString()
        {
            ServiceInfo info = null;
            var conn = ConfigurationManager.ConnectionStrings["AzureSearch"];
            if (conn != null)
            {
                try
                {
                    var parts = conn.ConnectionString
                        .Split(';')
                        .Select(p => p.Split('='))
                        .Select(t => new { key = t[0], value = t[1] }).ToList();

                    var service = parts.FirstOrDefault(p => p.key.Equals("ServiceName"));
                    var key = parts.FirstOrDefault(p => p.key.Equals("AdminKey"));
                    if (service != null && key != null)
                    {
                        info = new ServiceInfo
                        {
                            ServiceName = service.value.Trim(),
                            QueryKey = key.value.Trim()
                        };
                    }
                }
                catch
                {
                    //no need to get all null-checky and collection count happy... if 
                    //something went wrong above, then the connection string existed, but 
                    //is configured incorrectly
                    throw new ConfigurationErrorsException(
                        "The AzureSearch connection string is invalid, or incorrectly formatted");
                }
            }
            return info;
        }

        private class ServiceInfo
        {
            internal string ServiceName { get; set; }
            internal string QueryKey { get; set; }
        }
    }
}
