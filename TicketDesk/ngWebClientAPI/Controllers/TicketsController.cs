using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace ngWebClientAPI.Controllers
{
    public class TicketsController : ApiController
    {
        private TicketController ticketController = new TicketController(new TdDomainContext());

        private TdDomainContext Context { get; set; }

        // GET api/tickets
        public IEnumerable<string> Get()
        {
            
            return new string[] { "value1", "value2" };
        }

        // GET api/tickes/5
        public async Task<string> Get(int id)
        {
            string model = await ticketController.getTicket(id);
            return model;
        }

        // POST api/tickets
        public async Task<Boolean> Post([FromBody]Ticket value)
        {
            bool status = await ticketController.CreateTicketAsync(value);
            return status;
        }

        // PUT api/tickets/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tickets/5
        public void Delete(int id)
        {
        }
    }
}