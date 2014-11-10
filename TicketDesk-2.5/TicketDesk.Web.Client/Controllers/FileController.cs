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
        [HttpPost]
        public ActionResult Upload(Guid tempId)
        {
            var dir = AppDomain.CurrentDomain.GetData("DataDirectory");
            var attachmentDir = Path.Combine(dir.ToString(), "attachments");

            if (!Directory.Exists(attachmentDir))
            {
                Directory.CreateDirectory(attachmentDir);
            }
            foreach (string fileName in Request.Files)
            {
                var file = Request.Files[fileName];
                file.SaveAs(Path.Combine(attachmentDir, "file_" + tempId.ToString() + "_" + file.FileName));
            }

            return Json(new { Message = string.Empty });
        }

        [HttpPost]
        public ActionResult Delete(Guid tempId, string fileName)
        {
            var dir = AppDomain.CurrentDomain.GetData("DataDirectory");
            var attachmentDir = Path.Combine(dir.ToString(), "attachments");
            var fPath = Path.Combine(attachmentDir, "file_" + tempId.ToString() + "_" + fileName);
            if (System.IO.File.Exists(fPath))
            {
                System.IO.File.Delete(fPath);
            }

            return Json(new { Message = string.Empty });
        }
    }
}