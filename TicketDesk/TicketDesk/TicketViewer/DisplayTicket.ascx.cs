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
using System.Web.UI;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace TicketDesk.TicketViewer
{
    public partial class DisplayTicket : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate TicketChanged;

       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (TicketToDisplay != null)
            {
                TicketEditorControl.TicketToDisplay = TicketToDisplay;
                TicketEditorControl.TicketEditCompleted += new TicketPropertyChangedDelegate(TicketEditorControl_TicketEditCompleted);

                TicketActivityEditorControl.TicketToDisplay = TicketToDisplay;
                TicketActivityEditorControl.TicketActivityCompleted += new TicketPropertyChangedDelegate(TicketActivityEditorControl_TicketActivityCompleted);
                TicketActivityEditorControl.TicketActivityCanceled += new EventHandler(TicketActivityEditorControl_TicketActivityCanceled);
                TicketActivityEditorControl.TicketActivityFailed += new EventHandler(TicketActivityEditorControl_TicketActivityFailed);

                TicketAttachmentsControl.TicketToDisplay = TicketToDisplay;
               
                Page.Title = string.Format("({2}) {0}: {1}", TicketToDisplay.Type, TicketToDisplay.Title, TicketToDisplay.TicketId.ToString());

                if (!Page.IsPostBack)
                {
                    PopulateDisplay();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            TicketAttachmentsControl.Visible = (TicketActivityEditorControl.Activity != "AddAttachments");

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

        private void PopulateDisplay()
        {
            if (TicketToDisplay != null)
            {
                DisplayActivityButtons();

                TicketId.Text = TicketToDisplay.TicketId.ToString();

                TicketTitle.Text = TicketToDisplay.Title;

                if (TicketToDisplay.IsHtml)
                {
                    Details.Text = TicketToDisplay.Details;
                }
                else
                {
                    Details.Text = TicketToDisplay.Details.FormatAsHtml();
                }

                Category.Text = TicketToDisplay.Category;

                TicketType.Text = TicketToDisplay.Type;

                CreatedBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CreatedBy);

                CreatedDate.Text = TicketToDisplay.CreatedDate.ToString("g");

                Owner.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.Owner);

                AssignedTo.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo);

                CurrentStatus.Text = TicketToDisplay.CurrentStatus;

                CurrentStatusBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CurrentStatusSetBy);

                CurrentStatusDate.Text = TicketToDisplay.CurrentStatusDate.ToString("g");

                LastUpdateBy.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.LastUpdateBy);

                LastUpdateDate.Text = TicketToDisplay.LastUpdateDate.ToString("g");

                Priority.Text = TicketToDisplay.Priority;

                AffectsCustomer.Text = (TicketToDisplay.AffectsCustomer) ? "Yes" : "No";

                TicketAttachmentsControl.Refresh();

                
                var Tags = from t in TicketToDisplay.TicketTags
                           select new
                           {
                               Url = string.Format("~/TicketCenter.aspx?View=tagsandcategories&TagName={0}", t.TagName),
                               TagName = t.TagName
                           };
                TagRepeater.DataSource = Tags;
                TagRepeater.DataBind();

                CommentLogRepeater.DataSource = TicketToDisplay.TicketComments.OrderByDescending(tc => tc.CommentedDate);
                CommentLogRepeater.DataBind();
            }
        }

        void TicketPropertyChanged(TicketComment eventComment)
        {
            if (TicketChanged != null)
            {
                TicketChanged(eventComment);
            }
            PopulateDisplay();

        }

        protected string GetCommentHeadClass(string commentBy)
        {
            string returnClass = "CommentHead";
            if (commentBy == Page.User.Identity.GetFormattedUserName())
            {
                returnClass = "UserCommentHead";
            }
            return returnClass;
        }

        protected void ActivityButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            OpenActivityPanel();
            TicketActivityEditorControl.Activity = btn.CommandArgument;
            if (btn.CommandArgument == "EditTicket")
            {
                OpenTicketEditor();
            }


        }



        void TicketEditorControl_TicketEditCompleted(TicketComment eventComment)
        {
            CloseTicketEditor();
        }

        void TicketActivityEditorControl_TicketActivityCompleted(TicketComment eventComment)
        {

            if (TicketActivityEditorControl.Activity == "EditTicket")
            {
                if (!TicketEditorControl.Save(eventComment))
                {
                    TicketActivityEditorControl.Activity = "NoChanges";
                    CloseTicketEditor();
                    return;
                }
            }


            TicketToDisplay.TicketComments.Add(eventComment);
            TicketPropertyChanged(eventComment);
            CloseActivityPanel();
            PopulateDisplay();
            
        }

        void TicketActivityEditorControl_TicketActivityCanceled(object sender, EventArgs e)
        {
            if (TicketActivityEditorControl.Activity == "EditTicket")
            {
                CloseTicketEditor();
            }
            CloseActivityPanel();
            PopulateDisplay();
        }

        void TicketActivityEditorControl_TicketActivityFailed(object sender, EventArgs e)
        {

        }

        #region PanelVisibility

        private void OpenTicketEditor()
        {

            EditTicket_CollapsiblePanelExtender.ClientState = "false";
            EditTicket_CollapsiblePanelExtender.Collapsed = false;
            ViewTicket_CollapsiblePanelExtender.ClientState = "true";
            ViewTicket_CollapsiblePanelExtender.Collapsed = true;
        }

        private void OpenActivityPanel()
        {

            ActivityPanel_CollapsiblePanelExtender.ClientState = "false";
            ActivityPanel_CollapsiblePanelExtender.Collapsed = false;
            ActivityButtonPanel_CollapsiblePanelExtender.ClientState = "true";
            ActivityButtonPanel_CollapsiblePanelExtender.Collapsed = true;
        }


        private void CloseTicketEditor()
        {
            EditTicket_CollapsiblePanelExtender.ClientState = "true";
            EditTicket_CollapsiblePanelExtender.Collapsed = true;
            ViewTicket_CollapsiblePanelExtender.ClientState = "false";
            ViewTicket_CollapsiblePanelExtender.Collapsed = false;

        }

        private void CloseActivityPanel()
        {
            ActivityPanel_CollapsiblePanelExtender.ClientState = "true";
            ActivityPanel_CollapsiblePanelExtender.Collapsed = true;
            ActivityButtonPanel_CollapsiblePanelExtender.ClientState = "false";
            ActivityButtonPanel_CollapsiblePanelExtender.Collapsed = false;

        }

        #endregion

        private void DisplayActivityButtons()
        {
            EditTicketButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("EditTicket", Page.User.Identity.GetFormattedUserName());
            AddAttachementsButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("AddAttachments", Page.User.Identity.GetFormattedUserName());
            AddCommentButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("AddComment", Page.User.Identity.GetFormattedUserName());
            SupplyMoreInfoButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("SupplyInfo", Page.User.Identity.GetFormattedUserName());
            ResolveButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("Resolve", Page.User.Identity.GetFormattedUserName());
            RequestMoreInfoButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("RequestMoreInfo", Page.User.Identity.GetFormattedUserName());
            CancelMoreInfoButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("CancelMoreInfo", Page.User.Identity.GetFormattedUserName());
            CloseTicketButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("CloseTicket", Page.User.Identity.GetFormattedUserName());
            ReopenTicketButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("ReopenTicket", Page.User.Identity.GetFormattedUserName());
            TakeOverButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("TakeOver", Page.User.Identity.GetFormattedUserName());
            AssignButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("Assign", Page.User.Identity.GetFormattedUserName());
            GiveUpButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("GiveUp", Page.User.Identity.GetFormattedUserName());
            ForceCloseButton.Visible = TicketToDisplay.CheckSecurityForTicketActivity("ForceClose", Page.User.Identity.GetFormattedUserName());

            //TODO: Determine which ticket editor fields are editable based on user and security.
        }

    }
}