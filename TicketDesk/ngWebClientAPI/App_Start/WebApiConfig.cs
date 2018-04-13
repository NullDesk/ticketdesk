using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;


namespace ngWebClientAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            /*Web API configuration and serviers.*/
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            /*Enable attribute routing.*/
            config.MapHttpAttributeRoutes();
           
            /*Traditional routing also enabled.*/
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            /*Enable any HTTP request for any controller from the ngWebClient front end.*/
            var cors = new EnableCorsAttribute("http://localhost:4200,http://localhost:4201,http://localhost:4222", "*", "*") { SupportsCredentials = true };
            config.EnableCors(cors);

           // config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(DependencyInjectionCoreSetup._Container);
        }
    }
}
