using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Postal;
using S22.Mail;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Controllers
{
    public class EmailTestController : Controller
    {
        public TdDomainContext Context { get; set; }
        public EmailTestController(TdDomainContext context)
        {
            Context = context;
        }

        // GET: EmailTest
        public ActionResult Index(int id)
        {
            //TODO: Remove/mode to admin
            var ticket = Context.Tickets.Find(id);
            var email = new TicketEmail {Ticket = ticket};
            
         

            return new EmailViewResult(email);
        }
    }

    public class TicketEmail : Email
    {
        public Ticket Ticket { get; set; }
    }
    
}