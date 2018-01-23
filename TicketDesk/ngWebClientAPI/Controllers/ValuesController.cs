using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ngWebClientAPI.Controllers;


using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;

using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.IO;
using TicketDesk.Localization.Controllers;
using System.Data.Entity.Migrations;
using Newtonsoft.Json;

namespace ngWebClientAPI.Controllers
{
    public class ValuesController : ApiController
    {

        private TdDomainContext Context { get; set; }

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public async Task<string> Get(int id)
        {
      
            TicketController ticketController = new TicketController(new TdDomainContext());
            string model = await ticketController.getTicket(id);
            return model;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
