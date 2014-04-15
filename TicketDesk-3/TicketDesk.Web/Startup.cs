using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(TicketDesk.Web.Startup))]
namespace TicketDesk.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
