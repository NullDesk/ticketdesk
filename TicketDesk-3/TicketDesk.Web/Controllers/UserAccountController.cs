using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using TicketDesk.Domain.Identity;

namespace TicketDesk.Web.Controllers
{
    public class UserAccountController : ApiController
    {
        public UserAccountController()
        {
            IdentityManager = new AuthenticationIdentityManager(new IdentityStore(new TicketDeskIdentityContext()));
        }
        public System.Web.HttpContextWrapper WebContext
        {
            get
            {
                return (System.Web.HttpContextWrapper)ControllerContext.Request.Properties["MS_HttpContext"];
            }
        }
        public AuthenticationIdentityManager IdentityManager { get; private set; }

        private Microsoft.Owin.Security.IAuthenticationManager AuthenticationManager
        {
            get
            {
                //smr - this is pure voodoo here! Found this hackish way to get an MVC HttpContext in Web Api from the debugger.
                //TODO: This is probably not necessary once we move to the web api 2 tech stack, but until then...
                IDictionary<string, object> environment = (IDictionary<string, object>)(WebContext).Items["owin.Environment"];
                var owinCtx = new Microsoft.Owin.OwinContext(environment);
                return owinCtx.Authentication;
            }
        }

        [HttpPost]
        [ActionName("LoginDummy")]
        public HttpResponseMessage LoginDummy(Credential credential)
        {
            //this is just a dummy location for faking ajax login 
            //  forms in iframes --so browsers will remember passwords
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpPost]
        [ActionName("Login")]
        public async Task<HttpResponseMessage> Login(Credential credential)
        {
            await Seed();

            IdentityResult result = await IdentityManager.Authentication.CheckPasswordAndSignInAsync(AuthenticationManager, credential.Username, credential.Password, false);
            HttpStatusCode code = HttpStatusCode.OK;
            object content = null;
            if (result.Success)
            {
                content = new { Result = "true" };
            }
            else
            {
                code = HttpStatusCode.Unauthorized;
                content = new { Result = result.Errors.ToString() };
            }

            var jsonFormatter = Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            var response = Request.CreateResponse(code, content, jsonFormatter);
            return response;
        }

        [HttpGet]
        [Authorize]
        [ActionName("AuthenticationCheck")]
        public HttpResponseMessage AuthenticationCheck()
        {
            //just return ok, the real work was done by the Authorize attribute
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        

        [HttpGet]
        [Authorize]
        [ActionName("Logout")]
        public HttpResponseMessage Logout()
        {
           
            AuthenticationManager.SignOut();
            var respMessage = new HttpResponseMessage(HttpStatusCode.OK);
            var reqcookies = Request.Headers.GetCookies();
            foreach (var c in reqcookies)
            {
                if(c.Cookies.Any(ck => ck.Name == ".Aspnet.Application"))
                {
                    c.Expires = DateTimeOffset.Now.AddDays(-1);
                    respMessage.Headers.AddCookies(new CookieHeaderValue[] { c });
                    break;
                }
            }
            
            
            return respMessage;
        }

        private async Task Seed()
        {
            if (!IdentityManager.Logins.HasLocalLogin("admin"))
            {
                var result = await IdentityManager.Users.CreateLocalUserAsync(new TdUser() { UserName = "admin" }, "password");
                var x = result.Errors;
            }
        }
    }

    public class Credential
    {
        
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}