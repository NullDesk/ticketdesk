using System.Collections;
using System.Dynamic;
using System.Globalization;
using System.Net.Http.Formatting;
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
using Newtonsoft.Json.Serialization;
using TicketDesk.Web.Models.Localization;

namespace TicketDesk.Web.Controllers
{
    public class TextController : ApiController
    {
        // GET api/<controller>/en-us/appuitext
        public HttpResponseMessage Get(string lang, string ns)
        {
            var cinfo = lang.Equals("dev", StringComparison.InvariantCultureIgnoreCase) ?
                CultureInfo.InvariantCulture : new CultureInfo(lang);

            ICollection<KeyValuePair<string, object>> content = null;

            switch (ns)
            {
                case "appuitext":
                    content = GetTextResourceContent(AppUIText.ResourceManager.GetResourceSet(cinfo, true, true));
                    break;
                default:
                    break;
            }

            var jsonFormatter = Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return Request.CreateResponse(HttpStatusCode.OK, content, jsonFormatter); //Configuration.Formatters.JsonFormatter);
        }

        private ICollection<KeyValuePair<string, object>> GetTextResourceContent(ResourceSet set)
        {
            var resourceDictionary = set.Cast<DictionaryEntry>()
                                        .ToDictionary(r => r.Key.ToString(),
                                                      r => r.Value.ToString());
            var content = (ICollection<KeyValuePair<string, object>>) new ExpandoObject();

            foreach (var item in resourceDictionary)
            {
                content.Add(new KeyValuePair<string, object>(item.Key, item.Value));
            }
            return content;
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