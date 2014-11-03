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
using TicketDesk.Domain.Models;


namespace TicketDesk.Web.Client.Helpers
{
    public class TicketWithAttachmentsModelBinder : AttachmentsModelBinder
    {
        #region IModelBinder Members

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var Ticket = (Ticket)BindFromDefaultModelBinder(controllerContext, bindingContext);

            List<TicketAttachment> files = GetFilesFromForm(controllerContext);

            foreach (var file in files)
            {
                Ticket.TicketAttachments.Add(file);
            }

            return Ticket;
        }

       
        //public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    var request = controllerContext.HttpContext.Request;

        //    var formElements = new Dictionary<string, object>();
        //    request.Form.CopyTo(formElements);

        //    List<AttachmentFile> files = new List<AttachmentFile>();

        //    var fileIdElements = formElements.Where(fe => fe.Key.StartsWith("fileId_") || fe.Key.StartsWith("newFileId_"));
        //    foreach (var fileIdElement in fileIdElements)
        //    {
        //        string fileId = fileIdElement.Value as string;
        //        var fileNamePair = formElements.SingleOrDefault(fe => fe.Key == "fileName_" + fileId || fe.Key == "newFileName_" + fileId);
        //        var fileDescPair = formElements.SingleOrDefault(fe => fe.Key == "fileDescription_" + fileId || fe.Key == "newFileDescription_" + fileId);
        //        if (!string.IsNullOrEmpty(fileNamePair.Key) && !string.IsNullOrEmpty(fileNamePair.Key))
        //        {
        //            string fileName = fileNamePair.Value as string;
        //            string fileDescription = fileDescPair.Value as string;

        //            files.Add(new AttachmentFile { ExistingFileId = Convert.ToInt32(fileId), Name = fileName, Description = fileDescription });
        //        }
        //    }

        //    return files;
        //}

        #endregion
    }
}