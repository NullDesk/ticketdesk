using System.Globalization;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Resources;

namespace TicketDesk.Web.Controllers
{
    public class TextController : ApiController
    {
        // GET api/<controller>/en-us/appuitext
        public HttpResponseMessage Get(string lang, string ns)
        {
            CultureInfo ci = lang.Equals("dev", StringComparison.InvariantCultureIgnoreCase) ?
                CultureInfo.InvariantCulture : new CultureInfo(lang);

            Thread.CurrentThread.CurrentUICulture = ci;
            object content = null;
            switch (ns)
            {
                case "appuitext":
                    content = new
                        {
                            localizeMe = TicketDesk.Web.Models.Localization.AppUIText.LocalizeMe,
                            start = new { TicketDesk.Web.Models.Localization.AppUIText.Start_ScopedString },
                            displayName = TicketDesk.Web.Models.Localization.AppUIText.DisplayName
                        };
                    break;
                default:
                    break;
            }
            return Request.CreateResponse(HttpStatusCode.OK, content, Configuration.Formatters.JsonFormatter);
        }

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}