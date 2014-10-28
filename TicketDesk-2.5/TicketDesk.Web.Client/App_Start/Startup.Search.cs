using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model.Search;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureSearch()
        {
            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            if (DatabaseConfig.IsDatabaseReady)
            {
                context.SearchIndexer.RebuildIndexAsync();//don't await this, it'll run in the background
            }
        }
    }
}