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

using System.Configuration;
using System.Linq;
using RedDog.Search;
using RedDog.Search.Http;

namespace TicketDesk.Search.Azure
{
    public abstract class AzureSearchConector
    {
        private ApiConnection _connection;
        private IndexManagementClient _managementClient;
        private IndexQueryClient _queryClient;

        internal IndexManagementClient ManagementClient
        {
            get { return _managementClient ?? (_managementClient = new IndexManagementClient(Connection)); }
        }

        internal IndexQueryClient QueryClient
        {
            get { return _queryClient ?? (_queryClient = new IndexQueryClient(Connection)); }
        }

        internal ApiConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    var svcInfo = TryGetInfoFromConnectionString() ??
                                  TryGetInfoFromAppSettings();

                    _connection = ApiConnection.Create(svcInfo.ServiceName, svcInfo.QueryKey);
                }
                return _connection;
            }
        }

        public static ServiceInfo TryGetInfoFromAppSettings()
        {
            ServiceInfo info = null;
            var service = ConfigurationManager.AppSettings["Azure.Search.ServiceName"];
            var key = ConfigurationManager.AppSettings["Azure.Search.ApiKey"];
            if (service != null && key != null)
            {
                info = new ServiceInfo { ServiceName = service, QueryKey = key };
            }
            return info;
        }

        public static ServiceInfo TryGetInfoFromConnectionString()
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
                    var key = parts.FirstOrDefault(p => p.key.Equals("ApiKey"));
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

        public class ServiceInfo
        {
            public string ServiceName { get; set; }
            internal string QueryKey { get; set; }
        }
    }
}
