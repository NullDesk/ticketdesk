using System;
using System.Threading.Tasks;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Net;
using ngWebClientAPI.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.RoutePrefix("api/ticket")]
    public class TicketAPIController : ApiController
    {
        private TicketController ticketController = new TicketController(new TdDomainContext());

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
        public async Task<JObject> getAllTickets()
        {
            try
            {
                var model = await ticketController.GetTicketList(); //returns list of all tickets
                List<JObject> TicketList = new List<JObject>();
                foreach(var item in model)
                {
                    TicketList.Add(APITicketConversion.ConvertGETTicket(item));
                }
                return JObject.FromObject(TicketList);
            }
            catch(Exception ex)
            {
                return JObject.FromObject(ex.Message);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{ticketId}")]
        public async Task<JObject> getSingleTicket(int ticketId)
        {
            HttpStatusCodeResult result;
            /*try
            {
                Ticket model = await ticketController.getTicket(ticketId);
                result = new HttpStatusCodeResult(HttpStatusCode.OK, JsonConvert.SerializeObject(model));
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }*/
            Ticket model = await ticketController.getTicket(ticketId);
            //result = new HttpStatusCodeResult(HttpStatusCode.OK, JsonConvert.SerializeObject(model));
            //return JsonConvert.SerializeObject(model);
            if (model == null)
            {
                return null;
            }
            try
            {
                JObject retVal = APITicketConversion.ConvertGETTicket(model);

                return retVal;
            }
            catch
            {
                return null;
            }
            
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("")]
        public async Task<int> createTicket([FromBody]JObject jsonData)
        {
            Ticket ticket = APITicketConversion.ConvertPOSTTicket(jsonData);
            bool status = await ticketController.CreateTicketAsync(ticket);
            return 1;
        }
    }
}