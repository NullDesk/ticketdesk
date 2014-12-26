using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureSearch()
        {

            var context = DependencyResolver.Current.GetService<TicketDeskContext>();

            context.SearchProvider.InitializeSearch().ConfigureAwait(false);//don't await this, it'll run in the background

        }
    }
}