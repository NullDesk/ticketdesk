using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Domain.Model.Search;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureSearch()
        {
            var search = DependencyResolver.Current.GetService<SearchIndexer>();
            search.GenerateIndexAsync();//don't await this, it'll run in the background
        }
    }
}