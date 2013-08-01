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
using TicketDesk.Domain.Model.Localization;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

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
                    content = GetTextResourceContent(AppUIText.ResourceManager.GetResourceSet(cinfo, true, true), lang);
                    break;
                case "appmodeltext":
                    content = GetTextResourceContent(AppModelText.ResourceManager.GetResourceSet(cinfo, true, true), lang);
                    AddPrioritySettingList(lang, content);
                    AddTicketTypeSettingList(lang, content);
                    AddCategorySettingList(lang, content);
                    break;
                default:
                    break;
            }

            var jsonFormatter = Configuration.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return Request.CreateResponse(HttpStatusCode.OK, content, jsonFormatter); //Configuration.Formatters.JsonFormatter);
        }

        private ICollection<KeyValuePair<string, object>> GetTextResourceContent(ResourceSet set, string lang)
        {
            var resourceDictionary = set.Cast<DictionaryEntry>()
                                        .ToDictionary(r => r.Key.ToString(),
                                                      r => r.Value.ToString());
            var content = (ICollection<KeyValuePair<string, object>>)new ExpandoObject();

            foreach (var item in resourceDictionary)
            {
                content.Add(new KeyValuePair<string, object>(item.Key, item.Value));
            }

            return content;
        }

        private static void AddPrioritySettingList(string lang, ICollection<KeyValuePair<string, object>> content)
        {
            using (var ctx = new TicketDeskContext())
            {
                var baseItem = ctx.Settings.GetAvailablePriorities().ToArray();
                var localItem = ctx.Settings.GetAvailablePriorities(lang).ToArray();
                for (int i = 0; i < localItem.Count(); i++)
                {
                    var p = localItem[i];
                    var b = baseItem[i];
                    content.Add(new KeyValuePair<string, object>("Priority-" + b.Value, p.Value));
                }
            }
        }
        private static void AddTicketTypeSettingList(string lang, ICollection<KeyValuePair<string, object>> content)
        {
            using (var ctx = new TicketDeskContext())
            {
                var baseItem = ctx.Settings.GetAvailableTicketTypes().ToArray();
                var localItem = ctx.Settings.GetAvailableTicketTypes(lang).ToArray();
                for (int i = 0; i < localItem.Count(); i++)
                {
                    var p = localItem[i];
                    var b = baseItem[i];
                    content.Add(new KeyValuePair<string, object>("TicketType-" + b.Value, p.Value));
                }
            }
        }
        private static void AddCategorySettingList(string lang, ICollection<KeyValuePair<string, object>> content)
        {
            using (var ctx = new TicketDeskContext())
            {
                var baseItem = ctx.Settings.GetAvailableCategories().ToArray();
                var localItem = ctx.Settings.GetAvailableCategories(lang).ToArray();
                for (int i = 0; i < localItem.Count(); i++)
                {
                    var p = localItem[i];
                    var b = baseItem[i];
                    content.Add(new KeyValuePair<string, object>("Category-" + b.Value, p.Value));
                }
            }
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