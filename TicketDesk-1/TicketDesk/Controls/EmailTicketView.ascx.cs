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
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;


namespace TicketDesk.Controls
{
    public partial class EmailTicketView : System.Web.UI.UserControl
    {

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

       

        public void Populate(string linkUrl)
        {
            if(TicketToDisplay != null)
            {

                TicketAttachmentsDataSource.WhereParameters["TicketId"].DefaultValue = TicketToDisplay.TicketId.ToString();
                AttachmentsRepeater.DataSourceID = "TicketAttachmentsDataSource";
                AttachmentsRepeater.DataBind();
                TicketId.Text = TicketToDisplay.TicketId.ToString();
                TicketId.NavigateUrl = linkUrl;
                TicketTitle.Text = TicketToDisplay.Title;
                TicketTitle.NavigateUrl = linkUrl;
                if(TicketToDisplay.IsHtml)
                {
                    Details.Text = TicketToDisplay.Details;
                }
                else
                {
                    var md = new Markdown();
                    Details.Text = md.Transform(TicketToDisplay.Details, true);
                }
                Category.Text = TicketToDisplay.Category;
                //Category.NavigateUrl = string.Format("~/TicketCenter.aspx?View=tagsandcategories&Category={0}", TicketToDisplay.Category);
                TicketType.Text = TicketToDisplay.Type;
                TicketType.NavigateUrl = linkUrl;
                CreatedBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CreatedBy);
                CreatedDate.Text = TicketToDisplay.CreatedDate.ToString("g");
                Owner.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.Owner);
                AssignedTo.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo);
                //AssignedTo.NavigateUrl = string.Format("~/TicketCenter.aspx?View=assigned&User={0}", TicketToDisplay.AssignedTo);
                CurrentStatus.Text = TicketToDisplay.CurrentStatus;
                //CurrentStatus.NavigateUrl = string.Format("~/TicketCenter.aspx?View=status&Status={0}", TicketToDisplay.CurrentStatus.Replace(" ", string.Empty).ToLowerInvariant());
                CurrentStatusBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CurrentStatusSetBy);
                CurrentStatusDate.Text = TicketToDisplay.CurrentStatusDate.ToString("g");
                LastUpdateBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.LastUpdateBy);
                LastUpdateDate.Text = TicketToDisplay.LastUpdateDate.ToString("g");
                Priority.Text = TicketToDisplay.Priority;

                AffectsCustomer.Text = (TicketToDisplay.AffectsCustomer) ? "Yes" : "No";
                PublishedToKb.Text = (TicketToDisplay.PublishedToKb) ? "Yes" : "No";

                var Tags = from t in TicketToDisplay.TicketTags
                           select new
                           {
                               //Url = string.Format("~/TicketCenter.aspx?View=tagsandcategories&TagName={0}", t.TagName),
                               TagName = t.TagName
                           };
                TagRepeater.DataSource = Tags;
                TagRepeater.DataBind();

                CommentLogRepeater.DataSource = TicketToDisplay.TicketComments.OrderByDescending(tc => tc.CommentedDate);
                CommentLogRepeater.DataBind();
            }
            
            
        }
    }
}