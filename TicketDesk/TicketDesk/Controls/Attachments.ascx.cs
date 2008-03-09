using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class Attachments : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate AttachmentAdded;
        public event TicketAttachmentRemovedDelegate AttachmentRemoved;
        

        protected void Page_PreRender(object sender, EventArgs e)
        {
            AttachmentsUpdatePanel.Visible = (TicketToDisplay.CurrentStatus != "Closed");
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
                attachment.FileType = FileUploader.PostedFile.ContentType;
                attachment.FileContents = FileUploader.FileBytes;
                TicketToDisplay.TicketAttachments.Add(attachment);
                
                if(AttachmentAdded != null)
                {
                    AttachmentAdded();
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
                        AttachmentRemoved(attachment.FileId);
                    }

                    AttachmentsRepeater.DataBind();
                }
            }
        }
    }
}