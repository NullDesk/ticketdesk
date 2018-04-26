using System;
using System.Threading.Tasks;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using System.Web.Mvc;
using System.Linq;
using System.Net;
using ngWebClientAPI.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Configuration;

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
                var model = await ticketController.GetTicketList(); //returns list of all tickets
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
            int convertedId = APITicketConversion.ConvertTicketId(ticketId);//for when we get semantic numbering to front end
            Ticket model = await ticketController.getTicket(convertedId); //Expect full semantic id
            if (model == null)
            {
                return null; //Should probably error handle better here... Leaving as null for now
            }
            try
            {
                FrontEndTicket retVal = APITicketConversion.ConvertGETTicket(model);
                return JObject.FromObject(retVal);
            }
            catch
            {
                return null;
            }

        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("")]
        public async Task<JObject> createTicket([FromBody]JObject jsonData)
        {
            //make a new JObject to return to the front end
            POSTTicketResult result = new POSTTicketResult();
            //convert data to comment and ID
            Ticket ticket = new Ticket();
            try
            {
                /*This is going to be temporary.*/
                string userName = System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace(@"clarkpud\", string.Empty);

                ticket = APITicketConversion.ConvertPOSTTicket(jsonData, userName);
                bool status = await ticketController.CreateTicketAsync(ticket);

                if(status)
                {
                    //Successfully inserted new ticket to DB
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
            catch (Exception ex)
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
            int convertedId = APITicketConversion.ConvertTicketId(ticketId);//for when we get semantic numbering to front end
            Ticket model = await ticketController.getTicket(convertedId);
            if (model == null)
            {
                return null; // Should probably handle errors better here. Returning Null for now
            }
            try
            {
                EventList eventList = new EventList();
                //eventList.list = model.TicketEvents.ToList();
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