using System;
using System.Net;
using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using ngWebClientAPI.Models;
using Newtonsoft.Json.Linq;

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.Authorize]
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
                var model = await ticketController.GetTicketList();
                List<FrontEndTicket> TicketList = new List<FrontEndTicket>();
                foreach (var item in model)
                {
                    TicketList.Add(APITicketConversion.ConvertGETTicket(item));
                }
                JList lst = new JList();
                lst.list = TicketList;
                return JObject.FromObject(lst);
            }
            catch (Exception ex)
            {
                return JObject.FromObject(ex.Message);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{ticketId}")]
        public async Task<JObject> getSingleTicket(Int64 ticketId)
        {
            int convertedId = APITicketConversion.ConvertTicketId(ticketId);
            Ticket model = await ticketController.getTicket(convertedId); 

            try
            {
                FrontEndTicket retVal = APITicketConversion.ConvertGETTicket(model);
                return JObject.FromObject(retVal);
            }
            catch
            {
                throw new Exception("getSingleTicket : Could not convert ticketId. Ticket is null");

            }

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("")]
        public async Task<JObject> createTicket([FromBody]JObject jsonData)
        {
            POSTTicketResult result = new POSTTicketResult();
            Ticket ticket = new Ticket();

            try
            {
                string userName = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);
                ticket = APITicketConversion.ConvertPOSTTicket(jsonData, userName);
                bool status = await ticketController.CreateTicketAsync(ticket);

                if(status)
                {
                    result.httpCode = HttpStatusCode.OK;
                    result.ticketID = Int64.Parse(ticket.SemanticId + ticket.TicketId.ToString());
                    result.errorMessage = "";
                }
                else
                {
                    result.httpCode = HttpStatusCode.InternalServerError;
                    result.ticketID = -1;
                    result.errorMessage = "Internal Database Error";
                }
            }
            catch
            {
                result.httpCode = HttpStatusCode.BadRequest;
                result.ticketID = -1;
                result.errorMessage = "Malformed Ticket received: " + ticket.ToString();
            }
            return JObject.FromObject(result);
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("events/{ticketId}")]
        public async Task<JObject> GetEvents(Int64 ticketId)
        {
            int convertedId = APITicketConversion.ConvertTicketId(ticketId);
            Ticket model = await ticketController.getTicket(convertedId);

            try
            {
                EventList eventList = new EventList();
                List<TicketEvent> events = model.TicketEvents.ToList();
                eventList.list = new List<FrontEndEvent>();
                foreach(var item in events)
                {
                    eventList.list.Add(APITicketConversion.ConvertEvent(item));
                }
                return JObject.FromObject(eventList);
            }
            catch(Exception ex)
            {
                return JObject.FromObject(ex);
            }

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("categories")]
        public async Task<JObject> getCategories()
        {
            try
            {
                Dictionary<string, List<string>> dict = GlobalConfig.categories;
                return JObject.FromObject(dict);
            }
            catch (Exception ex)
            {
                return JObject.FromObject(ex);
            }
        }
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("priorities")]
        public async Task<JObject> getPriorities()
        {
            try
            {
                return JObject.FromObject(GlobalConfig.priorities);   
            }
            catch(Exception ex)
            {
                return JObject.FromObject(ex);
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("types")]
        public async Task<JObject> getTicketTypes()
        {
            try
            {
                return JObject.FromObject(GlobalConfig.ticketTypes);
            }
            catch(Exception ex)
            {
                return JObject.FromObject(ex);
            }
        }
    }
}