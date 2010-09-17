using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;
using TicketDesk.Domain.Services;

namespace TicketDesk.Web.Client.Controllers
{

    [Export("Attachment", typeof(IController))]
    public partial class AttachmentController : ApplicationController
    {

        private ITicketService Tickets { get; set; }

        [ImportingConstructor]
        public AttachmentController(ITicketService ticketService, ISecurityService security)
            : base(security)
        {
            Tickets = ticketService;
        }

        //
        // GET: /Downloader/
        [Authorize]
        public virtual ActionResult Download(int fileId)
        {

            var attachment = Tickets.GetAttachment(fileId);
            if (attachment != null)
            {
                return new FileContentResult(attachment.FileContents.ToArray(), attachment.FileType) { FileDownloadName = attachment.FileName };
            }
            else
            {
                return new EmptyResult();
            }
        }

    }
}
