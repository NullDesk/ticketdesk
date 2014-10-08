using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
