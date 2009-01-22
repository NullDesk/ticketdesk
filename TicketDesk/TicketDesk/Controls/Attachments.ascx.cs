// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Linq;
using System.Web.UI.WebControls;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class Attachments : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate AttachmentAdded;
        public event TicketAttachmentRemovedDelegate AttachmentRemoved;
        

        protected void Page_PreRender(object sender, EventArgs e)
        {
            AttachmentsUpdatePanel.Visible = (TicketToDisplay != null) && (TicketToDisplay.CurrentStatus != "Closed");
        }

        private Ticket _ticket;
        public Ticket TicketToDisplay
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public string GetAttachmentLinkUrl(int fileId)
        {
            return string.Format("~/DownloadAttachment.ashx?fileId={0}", fileId.ToString());
        }

        

        protected void UploadFile_Click(object sender, EventArgs e)
        {
            if(FileUploader.HasFile)
            {
                TicketComment comment = new TicketComment();
                comment.CommentEvent = "has added an attachment";
                comment.IsHtml = false;
                comment.Comment = string.Format("New file: {0}", FileUploader.FileName);
                TicketToDisplay.TicketComments.Add(comment);
                
                TicketAttachment attachment = new TicketAttachment();
                attachment.FileName = FileUploader.FileName;
                attachment.FileSize = FileUploader.PostedFile.ContentLength;
                attachment.FileType = (string.IsNullOrEmpty(FileUploader.PostedFile.ContentType)? "application/octet-stream" : FileUploader.PostedFile.ContentType);
                attachment.FileContents = FileUploader.FileBytes;
                TicketToDisplay.TicketAttachments.Add(attachment);
                
                if(AttachmentAdded != null)
                {
                    AttachmentAdded(comment);
                }
                AttachmentsRepeater.DataBind();
            }
        }

        protected void AttachmentsRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if(e.CommandName == "delete")
            {
                TicketAttachment attachment = TicketToDisplay.TicketAttachments.SingleOrDefault(a => a.FileId == Convert.ToInt32(e.CommandArgument));
                if(attachment != null)
                {
                    TicketComment comment = new TicketComment();
                    comment.CommentEvent = "has removed an attachment";
                    comment.IsHtml = false;
                    comment.Comment = string.Format("Removed file: {0}", attachment.FileName);
                    TicketToDisplay.TicketComments.Add(comment);
                    if(AttachmentRemoved != null)
                    {
                        AttachmentRemoved(attachment.FileId, comment);
                    }

                    AttachmentsRepeater.DataBind();
                }
            }
        }
    }
}