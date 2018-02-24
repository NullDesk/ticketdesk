using System;
using System.Threading.Tasks;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using Newtonsoft.Json;

namespace ngWebClientAPI.Controllers
{
    [RoutePrefix("api/ticket")]
    public class TicketAPIController : ApiController
    {
        private TicketController ticketController = new TicketController(new TdDomainContext());

        [HttpGet]
        [Route("")]
        public async Task<string> getAllTickets()
        {
            var model = await ticketController.GetTicketList(); //returns list of all tickets
            return JsonConvert.SerializeObject(model);
        }

        [HttpGet]
        [Route("{ticketId}")]
        public async Task<Ticket> getSingleTicket(int ticketId)
        {
            Ticket model = await ticketController.getTicket(ticketId);
            return model;
        }

        [HttpPost]
        [Route("")]
        public async Task<Boolean> createTicket([FromBody]Ticket value)
        {
            bool status = await ticketController.CreateTicketAsync(value);
            return status;
        }
    }
}