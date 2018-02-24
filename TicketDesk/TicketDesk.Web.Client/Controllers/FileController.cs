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

using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketDesk.IO;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("file")]
    [TdAuthorize(Roles = "TdInternalUsers,TdHelpDeskUsers,TdAdministrators")]
    public class FileController : Controller
    {
        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult> Upload(Guid tempId)
        {
            var demoMode = (ConfigurationManager.AppSettings["ticketdesk:DemoModeEnabled"] ?? "false").Equals("true", StringComparison.InvariantCultureIgnoreCase);
            if (demoMode)
            {
                return new EmptyResult();
            }
            //TODO: doesn't necessarily play well with large files, cap file upload size to something reasonable (define in settings) or use upload chunking
            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];
                Debug.Assert(file != null, "file != null");
                
                await TicketDeskFileStore.SaveAttachmentAsync(file.InputStream, file.FileName, tempId.ToString(), true);
            }
            return new JsonCamelCaseResult { Data = new { Message = string.Empty } };//dropzone expects a message property back
        }

        [HttpPost]
        [Route("delete")]
        public JsonResult Delete(Guid id, string fileName)
        {
            //ticketdesk never lets the UI directly delete a file attachment unless it is a pending. 
            TicketDeskFileStore.DeleteAttachment(fileName, id.ToString(), true);
            return new JsonCamelCaseResult { Data = new { Message = string.Empty } };//dropzone expects a message property back
        }

        [HttpGet]
        [Route("{id}/{fileName}", Name = "GetAttachedFile")]
        public ActionResult GetFile(string id, string fileName)
        {
            return FetchFile(fileName, id, IsFilePending(id));
        }

        [HttpGet]
        [Route("get-attachments-info")]
        public ActionResult GetAttachmentsInfo(int? id, Guid? tempId)
        {
            var pending = tempId.HasValue ? TicketDeskFileStore.ListAttachmentInfo(tempId.Value.ToString(), true) : new TicketDeskFileInfo[0];
            var attached = id.HasValue ? TicketDeskFileStore.ListAttachmentInfo(id.Value.ToString(CultureInfo.InvariantCulture), false) : new TicketDeskFileInfo[0];

            var files = pending.Select(f => tempId != null ? new
            {
                f.Name,
                f.Size,
                isAttached = false,
                Type = MimeTypeMap.GetMimeType(Path.GetExtension(f.Name)),
                Url = Url.RouteUrl("GetPendingFile", new { Id = tempId.Value, FileName = f.Name, })
            } : null)
            .Union(attached.Select(f => id != null ? new
            {
                f.Name,
                f.Size,
                isAttached = true,
                Type = MimeTypeMap.GetMimeType(Path.GetExtension(f.Name)),
                Url = Url.RouteUrl("GetAttachedFile", new { Id = id.Value, FileName = f.Name })
            } : null));


            return new JsonCamelCaseResult
            {
                Data = files,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private bool IsFilePending(string containerId)
        {
            Guid tId;
            return Guid.TryParse(containerId, out tId);
        }

        private ActionResult FetchFile(string fileName, string container, bool isPending)
        {
            var fstream = TicketDeskFileStore.GetFile(fileName, container, isPending);
            return new FileStreamResult(fstream, "application/octet-stream");//always send it back as octet-stream so client browser actually downloads it, instead of displaying it on-screen
        }
    }
}