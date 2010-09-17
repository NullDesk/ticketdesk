using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketDesk.Web.Client.Helpers;
using TicketDesk.Domain.Services;
using TicketDesk.Domain.Models;
using System.IO;
using System.ComponentModel.Composition;

namespace TicketDesk.Web.Client.Controllers
{
    [Export("Uploader", typeof(IController))]
    public partial class UploaderController : Controller
    {
        private ITicketService Tickets { get; set; }

        [ImportingConstructor]
        public UploaderController(ITicketService ticketService)
        {
            Tickets = ticketService;
        }

        [FlashCompatibleAuthorizeAttribute]
        public virtual ActionResult AddAttachment(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase userPostedFile = Request.Files[0];
                    {
                        var attachment = new TicketAttachment();

                        string fName = Path.GetFileName(userPostedFile.FileName);

                        attachment.FileName = fName;

                        attachment.FileSize = userPostedFile.ContentLength;
                        string mtype = userPostedFile.ContentType;
                        attachment.FileType = (string.IsNullOrEmpty(mtype) ? "application/octet-stream" : mtype);
                        byte[] fileContent = new byte[userPostedFile.ContentLength];
                        userPostedFile.InputStream.Read(fileContent, 0, userPostedFile.ContentLength);
                        if (System.Configuration.ConfigurationManager.AppSettings["IsDemo"] == "true")
                        {
                            attachment.FileContents = System.Text.Encoding.UTF8.GetBytes("The demo does not store upload content...");
                        }
                        else
                        {
                            attachment.FileContents = fileContent;
                        }
                        try
                        {

                            int fileId = Tickets.AddPendingAttachment(id, attachment);
                            return Content(fileId.ToString());
                        }
                        catch 
                        {
                            return new EmptyResult();
                        }


                    }
                }
                throw new InvalidOperationException("No file data was uploaded");
            }
            throw new InvalidOperationException("The user is not authenticated.");
        }


    }
}
