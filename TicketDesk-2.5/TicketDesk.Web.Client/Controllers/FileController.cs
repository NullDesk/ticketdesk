using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Controllers
{
    public class FileController : Controller
    {
        public FileController()
        {
            //ensure attachments location is ready
            var attachmentDir = GetTempAttachmentsFolder();

            if (!Directory.Exists(attachmentDir))
            {
                Directory.CreateDirectory(attachmentDir);
            }
        }

        [HttpPost]
        public ActionResult Upload(Guid tempId)
        {
            //TODO: doesn't necessarily play well with large files, cap file upload size to something reasonable (define in settings)
            foreach (string fileName in Request.Files)
            {
                //TODO: delegate to storage a provider
                var file = Request.Files[fileName];
                file.SaveAs(GetFilePath(tempId, file.FileName));
            }
            return Json(new { Message = string.Empty });//dropzone expects a message property back
        }

        [HttpPost]
        public ActionResult Delete(Guid tempId, string fileName)
        {
            //TODO: delegate to storage a provider
            var fPath = GetFilePath(tempId, fileName);
            if (System.IO.File.Exists(fPath))
            {
                System.IO.File.Delete(fPath);
            }
            return Json(new { Message = string.Empty });//dropzone expects a message property back
        }

        private string GetFilePath(Guid tempId, string fileName)
        {
            return Path.Combine(GetTempAttachmentsFolder(), string.Format("{1}.{0}", tempId.ToString(), fileName));
        }

        private string GetTempAttachmentsFolder()
        {
            //TODO: take location from storage settings
            var dir = AppDomain.CurrentDomain.GetData("DataDirectory");
            var attachmentDir = Path.Combine(dir.ToString(), "attachments", "pending");
            return attachmentDir;
        }
    }
}