using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedDog.Search;
using RedDog.Search.Http;
using RedDog.Search.Model;

namespace TicketDesk.Domain.Search
{

    public class IndexManager
    {
        private ApiConnection connection;
        private IndexManagementClient managementClient;
        private IndexQueryClient queryClient;

        public async void RemoveTicketIndex()
        {
            var result = await ManagementClient.DeleteIndexAsync("ticketdesk-tickets");
            if (!result.IsSuccess)
            {
                Trace.Write("Error: " + result.Error.Message);
            }
        }

        public async Task<bool> IndexExists()
        {
            var ret = false;
            var result = await ManagementClient.GetIndexesAsync();
            if (result.IsSuccess)
            {
                ret = result.Body.Any(i => i.Name.Equals("ticketdesk-tickets"));
            }
            else
            {
                Trace.Write("Error: " + result.Error.Message);
            }
            return ret;
        }

        public async void CreateTicketIndex()
        {
            var index = new Index("ticketdesk-tickets")
                .WithStringField("id", opt =>
                    opt.IsKey().IsRetrievable().IsSearchable().IsSortable().IsFilterable())
                .WithStringField("title", opt =>
                    opt.IsRetrievable().IsSearchable())
                .WithStringField("status", opt =>
                    opt.IsRetrievable().IsSearchable().IsSortable().IsFilterable())
                .WithDateTimeField("lastupdated", opt =>
                    opt.IsSortable().IsFilterable())
                .WithStringField("details", opt =>
                    opt.IsSearchable())
                .WithStringCollectionField("tags", opt =>
                    opt.IsRetrievable().IsSearchable().IsFilterable())
                .WithStringCollectionField("comments", opt =>
                    opt.IsSearchable());


            index.ScoringProfiles = new Collection<ScoringProfile>
            {
                new ScoringProfile()
                {
                    Name = "fieldBooster",
                    Text = new ScoringProfileText
                    {
                        Weights = new Dictionary<string, double>
                        {
                            {"id", 3D},
                            {"tags", 2.5D},
                            {"title", 2D},
                            {"comments", 0.75D}
                        }
                    },
                    Functions =
                    {
                        new ScoringProfileFunction
                        {
                            Type = ScoringProfileFunctionType.Freshness,
                            FieldName = "lastupdated",
                            Boost = 1.25d,
                            Interpolation = InterpolationType.Quadratic,
                            Freshness = new ScoringProfileFunctionFreshness()
                            {
                                BoostingDuration = new TimeSpan(5, 0, 0)
                            }
                        }
                    }
                }
            };
            index.DefaultScoringProfile = "fieldBooster";

            var result = await ManagementClient.CreateIndexAsync(index);

            if (!result.IsSuccess)
            {
                Trace.WriteLine("Error: " + result.Error.Message);
            }
        }


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
                            .Select(t => new { key = t[0], value = t[1] });

                    var service = parts.FirstOrDefault(p => p.key.Equals("ServiceName"));
                    var key = parts.FirstOrDefault(p => p.key.Equals("AdminKey"));
                    info = new ServiceInfo
                    {
                        ServiceName = service.value.Trim(),
                        QueryKey = key.value.Trim()
                    };
                }
                catch
                {
                    //no need to get all null-checky and collection count happy... if 
                    //something went wrong above, then the connection string existed, but 
                    //is configured incorrectly
                    throw new ConfigurationErrorsException("The AzureSearch connection string is invalid, or incorectly formatted");
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
