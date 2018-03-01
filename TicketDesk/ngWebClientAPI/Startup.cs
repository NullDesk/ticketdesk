using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(ngWebClientAPI.Startup))]

namespace ngWebClientAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //HttpConfiguration httpConfiguration = new HttpConfiguration();
            //WebApiConfig.Register(httpConfiguration);
            //app.UseWebApi(httpConfiguration);

        }
    }
}
