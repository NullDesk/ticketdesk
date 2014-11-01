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
using TicketDesk.Web.Client.Helpers;
using TicketDesk.Domain.Services;
using TicketDesk.Domain.Models;
using System.IO;
using System.ComponentModel.Composition;
using System.Configuration;

namespace TicketDesk.Web.Client.Controllers
{
    [Export("Uploader", typeof(IController))]
    public partial class UploaderController : Controller
    {
        private ITicketService Tickets { get; set; }
        private SettingsService Settings { get; set; }

        [ImportingConstructor]
        public UploaderController(ITicketService ticketService, SettingsService settingsService)
        {
            Settings = settingsService;
            Tickets = ticketService;
        }

        [Authorize]
        public virtual JsonResult AddAttachment(int? id)
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

                        var isDemo = (bool)Settings.ApplicationSettings.GetSettingValue("IsDemo");

                        if (isDemo)
                        {
                            attachment.FileContents = System.Text.Encoding.UTF8.GetBytes("The demo does not store upload content...");
                        }
                        else
                        {
                            attachment.FileContents = fileContent;
                        }
                        try
                        {
                            System.Threading.Thread.Sleep(2000);
                            int fileId = Tickets.AddPendingAttachment(id, attachment);
                            return new JsonResult() { ContentType = "text/plain", Data = new { success = true, id = fileId.ToString() } };
                        }
                        catch
                        {
                            return new JsonResult();
                        }


                    }
                }
                throw new InvalidOperationException("No file data was uploaded");
            }
            throw new InvalidOperationException("The user is not authenticated.");
        }


    }
}
