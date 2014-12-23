using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TicketDesk.IO;

namespace TicketDesk.Web.Client.Controllers
{
    [RoutePrefix("file")]
    public class FileController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Upload(Guid tempId)
        {
            //TODO: doesn't necessarily play well with large files, cap file upload size to something reasonable (define in settings) or use upload chunking
            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];
                Debug.Assert(file != null, "file != null");
                await TicketDeskFileStore.SaveAttachmentAsync(file.InputStream, file.FileName, tempId.ToString(), true);
            }
            return Json(new { Message = string.Empty });//dropzone expects a message property back
        }
       
        [HttpPost]
        public ActionResult Delete(Guid id, string fileName)
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


            return new JsonCamelCaseResult()
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