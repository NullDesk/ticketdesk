// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Postal;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Web.Client.Models;

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
            //TODO: Remove/move to admin
            var ticket = Context.Tickets.Include(t => t.TicketTags).First(t => t.TicketId == id);
            var root = Context.TicketDeskSettings.ClientSettings.GetDefaultSiteRootUrl();
            var email = new TicketEmail { Ticket = ticket, SiteRootUrl = root };

            //email.Send();
            //string content;
            //var mailService = new Postal.EmailService();
            //SerializableMailMessage message = mailService.CreateMailMessage(email);

            //var client = new SmtpClient()
            //{
            //    Host = "localhost",
            //    Port = 25
            //};

            //client.Send(message);
            ////serialize:
            //    using (var ms = new MemoryStream())
            //{
            //    new BinaryFormatter().Serialize(ms, message);
            //    content = Convert.ToBase64String(ms.ToArray());
            //}

            ////deserialize:

            //var memorydata = Convert.FromBase64String(content);
            //using (var rs = new MemoryStream(memorydata))
            //{
            //    var sf = new BinaryFormatter();
            //    var m = (SerializableMailMessage)sf.Deserialize(rs) ;
            //    client.Send(m);
            //}


            return new EmailViewResult(email);
        }
    }



}