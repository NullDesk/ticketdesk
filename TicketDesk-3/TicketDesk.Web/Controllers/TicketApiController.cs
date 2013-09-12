using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TicketDesk.Domain.Model;
using TicketDesk.Domain;
using Breeze.WebApi;
using TicketDesk.Web.Infrastructure;
using System.Web.Http.Controllers;
using System.Threading;

namespace TicketDesk.Web.Controllers
{


    [BreezeController]
    [Authorize]
    public class TicketApiController : ApiController
    {

        //private readonly TicketDeskContext _db = new TicketDeskContext();
        private readonly TicketDeskBreezeContext _contextProvider = new TicketDeskBreezeContext();

        [HttpGet]
        public string Metadata()
        {
            return _contextProvider.Metadata();
        }

        // GET api/Ticket
        [HttpGet]
        public IQueryable<Ticket> Tickets()
        {
            return _contextProvider.Context.Tickets;
        }

        [HttpGet]
        public IEnumerable<TicketTag> TagSuggestionList()
        {
            return _contextProvider.Context.TicketTags.GetTagList();
        }

        [HttpGet]
        public IEnumerable<SimpleSetting> PriorityList()
        {
            return _contextProvider.Context.Settings.GetAvailablePriorities();
        }

        [HttpGet]
        public IEnumerable<SimpleSetting> TicketTypeList()
        {
            return _contextProvider.Context.Settings.GetAvailableTicketTypes();
        }

        [HttpGet]
        public IEnumerable<SimpleSetting> CategoryList()
        {
            return _contextProvider.Context.Settings.GetAvailableCategories();
        }

        [HttpGet]
        public IEnumerable<SimpleSetting> StatusList()
        {
            return _contextProvider.Context.Settings.GetAvailableStatuses("en");
        }




        //// GET api/Ticket/5
        //[HttpGet]
        //public Ticket Ticket(int id)
        //{
        //    var ticket = _contextProvider.Context.Tickets.Find(id);
        //    if (ticket == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return ticket;
        //}

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _contextProvider.SaveChanges(saveBundle);
        }

        protected override void Dispose(bool disposing)
        {
            _contextProvider.Context.Dispose();
            base.Dispose(disposing);
        }
    }
}