using Owin;

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
