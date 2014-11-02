// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

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
