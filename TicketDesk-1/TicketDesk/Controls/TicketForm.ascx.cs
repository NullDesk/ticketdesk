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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.IO;

namespace TicketDesk.Controls
{
    public partial class TicketForm : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.Enctype = "multipart/form-data";
            lblDetailsRequired.Visible = false;
            if (!Page.IsPostBack)
            {

                bool EnableFullMode = (SecurityManager.IsStaffOrAdmin);

                TypeDropDownList.DataSource = SettingsManager.TicketTypesList;
                TypeDropDownList.DataBind();

                PriorityDropDownList.DataSource = SettingsManager.PrioritiesList;
                PriorityDropDownList.DataBind();

                CategoryDropDownList.DataSource = SettingsManager.CategoriesList;
                CategoryDropDownList.DataBind();

                PriorityDropDownList.Enabled = (SecurityManager.SubmitterCanEditPriority || EnableFullMode);


                OwnerUpdatePanel.Visible = EnableFullMode;

                OwnerDropDownList.Items.AddRange(GetOwnerUserList());
            }
        }

        public Ticket GetNewTicket()
        {
            string details = Page.Request.Form["details"];

            Ticket ticket = null;
            if (Page.IsValid)
            {
                if (!string.IsNullOrEmpty(details))
                {
                    DateTime now = DateTime.Now;
                    string user = Page.User.Identity.GetFormattedUserName();
                    ticket = new Ticket();
                    ticket.Type = TypeDropDownList.SelectedValue;
                    ticket.Category = CategoryDropDownList.SelectedValue;
                    ticket.Title = TitleTextBox.Text;
                    ticket.IsHtml = false;
                    ticket.Details = details;
                    if (!string.IsNullOrEmpty(PriorityDropDownList.SelectedValue))
                    {
                        ticket.Priority = PriorityDropDownList.SelectedValue;
                    }
                    string[] tags = TagManager.GetTagsFromString(TagPickerControl.TagList);
                    ticket.TagList = string.Join(",", tags);
                    ticket.AffectsCustomer = AffectsCustomerCheckBox.Checked;
                    ticket.PublishedToKb = false;
                    ticket.CreatedBy = user;
                    ticket.CreatedDate = now;
                    if (CreateOnBehalfTextBox.Checked)
                    {
                        ticket.Owner = OwnerDropDownList.SelectedValue;
                    }
                    else
                    {
                        ticket.Owner = user;
                    }
                    ticket.CurrentStatus = "Active";
                    ticket.CurrentStatusSetBy = user;
                    ticket.CurrentStatusDate = now;


                    TicketComment openingComment = new TicketComment();
                    if (CreateOnBehalfTextBox.Checked)
                    {
                        openingComment.CommentEvent = string.Format("created the ticket on behalf of {0}", SecurityManager.GetUserDisplayName(ticket.Owner));
                    }
                    else
                    {
                        openingComment.CommentEvent = string.Format("created the ticket");
                    }
                    openingComment.CommentedBy = user;
                    openingComment.CommentedDate = now;


                    HttpFileCollection uploadedFiles = Request.Files;

                    for (int i = 0; i < uploadedFiles.Count; i++)
                    {
                        
                        HttpPostedFile userPostedFile = uploadedFiles[i];
                        if (userPostedFile.ContentLength > 0)
                        {
                            TicketAttachment attachment = new TicketAttachment();
                            string fName = Path.GetFileName(userPostedFile.FileName);//FileUploader.FileName;

                            attachment.FileName = fName;
                            var description = Page.Request.Form[fName];
                            if (!string.IsNullOrEmpty(description))
                            {
                                if (description.Length > 500)
                                {
                                    attachment.FileDescription = description.Substring(0, 500);
                                }
                                else
                                {
                                    attachment.FileDescription = description;
                                }
                            }
                            attachment.FileSize = userPostedFile.ContentLength;//FileUploader.PostedFile.ContentLength;
                            string mtype = userPostedFile.ContentType;
                            attachment.FileType = (string.IsNullOrEmpty(mtype) ? "application/octet-stream" : mtype);
                            byte[] fileContent = new byte[userPostedFile.ContentLength];
                            userPostedFile.InputStream.Read(fileContent, 0, userPostedFile.ContentLength);//FileUploader.FileBytes;
                            attachment.FileContents = fileContent;
                            ticket.TicketAttachments.Add(attachment);
                        }
                    }

                    ticket.TicketComments.Add(openingComment);

                    foreach (string tag in tags)
                    {
                        TicketTag tTag = new TicketTag();
                        tTag.TagName = tag;
                        ticket.TicketTags.Add(tTag);
                    }
                }
                else
                {
                    lblDetailsRequired.Visible = true;
                }

            }
            return ticket;
        }

        public ListItem[] GetOwnerUserList()
        {
            User[] users = SecurityManager.GetTicketSubmitterUsers();
            var userItems = from user in users
                            where user.Name.ToUpperInvariant() != HttpContext.Current.User.Identity.Name.ToUpperInvariant()
                            select new ListItem(user.DisplayName, user.Name);

            return userItems.ToArray();
        }

        protected void CreateOnBehalfTextBox_CheckedChanged(object sender, EventArgs e)
        {
            OwnerPanel.Visible = CreateOnBehalfTextBox.Checked;
        }
    }
}