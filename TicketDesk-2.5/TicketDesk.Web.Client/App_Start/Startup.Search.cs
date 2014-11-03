using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureSearch()
        {
            //TODO: Any search setup needed (all that detecting and app settings stuff)
            //      Actual index rebuilding and maintenance needs to be done from SearchManager so it can run in other processes (like webjob, etc.)


            var context = DependencyResolver.Current.GetService<TicketDeskContext>();
            if (DatabaseConfig.IsDatabaseReady)
            {
                context.SearchManager.InitializeSearch().ConfigureAwait(false);//don't await this, it'll run in the background
            }
        }
    }
}