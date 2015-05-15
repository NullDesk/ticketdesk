using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Postal;
using RazorEngine.Compilation.ImpromptuInterface;
using TicketDesk.Domain.Model;

namespace TicketDesk.Postal
{
    public class TicketMailGenerator
    {
        private EmailService MailService { get; set; }
        public TicketMailGenerator(string templatesPath )
        {
            // Get the path to the directory containing views
            var viewsPath = Path.GetFullPath(templatesPath);//@"..\..\Views");

            var engines = new ViewEngineCollection {new FileSystemRazorViewEngine(viewsPath)};

            MailService = new EmailService(engines);
            
        }

        public string GenerateMessageForTicket(Ticket ticket)
        {
           throw new NotImplementedException();
        }

    }
}
