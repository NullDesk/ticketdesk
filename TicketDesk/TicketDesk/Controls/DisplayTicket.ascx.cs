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

namespace TicketDesk.Controls
{
    public partial class DisplayTicket : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate TicketChanged;
        public event TicketAttachmentRemovedDelegate TicketAttachmentRemoved;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(TicketToDisplay != null)
            {
                DisplayEditControls();
                DisplayActionControls();

                Page.Title = string.Format("({2}) {0}: {1}", TicketToDisplay.Type, TicketToDisplay.Title, TicketToDisplay.TicketId.ToString());

                if(!Page.IsPostBack)
                {
                    PopulateDisplay();
                }
            }
        }

        private void DisplayEditControls()
        {

            AddCommentControl.Visible = EnableEditControls;
            ChangeDetailsPopup.Visible = EnableEditControls;
            ChangeTagsPopupControl.Visible = EnableEditControls;
            ChangeCategoryPopupControl.Visible = EnableEditControls;
            ChangeTitleTypePopupControl.Visible = EnableEditControls;
            ChangeAffectsCustomerPopupControl.Visible = EnableEditControls;
            ChangeOwnedByPopupControl.Visible = EnableEditControls;
            
            AddCommentsContainer.Visible = EnableEditControls;
            
            if(EnableEditControls)
            {
                HookupEditControlEvents();
            }
        }

        private void DisplayActionControls()
        {
             bool isAssignedToMe = (!string.IsNullOrEmpty(TicketToDisplay.AssignedTo) && TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());
             AddCommentButton.Visible = EnableActionControls && TicketToDisplay.CurrentStatus != "Resolved" && TicketToDisplay.CurrentStatus != "Closed";
            AddCommentButton.Text = (TicketToDisplay.CurrentStatus == "More Info") ? "Supply Info" : "Add Comment";
            AssignPopupControl.Visible = EnableActionControls;
            ChangePriorityPopupControl.Visible = EnableActionControls;
            CancelMoreInfoPopupControl.Visible = EnableActionControls;
            RequestMoreInfoPopupControl.Visible = EnableActionControls;
            TakeOverPopupControl.Visible = EnableActionControls;
            GiveUpPopupControl.Visible = EnableActionControls && isAssignedToMe && TicketToDisplay.CurrentStatus != "Resolved" && TicketToDisplay.CurrentStatus != "Closed";
            CloseTicketPopupControl.Visible = EnableActionControls;
            ReOpenPopupControl.Visible = EnableActionControls;
            ForceCloseTicketPopupControl.Visible = EnableActionControls;
            ResolveButton.Visible = EnableActionControls && TicketToDisplay.CurrentStatus != "More Info" && TicketToDisplay.CurrentStatus != "Closed" && TicketToDisplay.CurrentStatus != "Resolved" && isAssignedToMe;
            if(EnableActionControls)
            {
                HookupActionControlEvents();
            }
        }

        private void HookupEditControlEvents()
        {
            ChangeOwnedByPopupControl.TicketToDisplay = TicketToDisplay;
            ChangeOwnedByPopupControl.OwnerChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ChangeCategoryPopupControl.TicketToDisplay = TicketToDisplay;
            ChangeCategoryPopupControl.CategoryChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ChangeTagsPopupControl.TicketToDisplay = TicketToDisplay;
            ChangeTagsPopupControl.TagsChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ChangeDetailsPopup.TicketToDisplay = TicketToDisplay;
            ChangeDetailsPopup.DetailsChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            AddCommentControl.TicketToDisplay = TicketToDisplay;
            AddCommentControl.CommentChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);
            AddCommentControl.CancelComment += new EventHandler(AddCommentControl_CancelComment);

            ChangeTitleTypePopupControl.TicketToDisplay = TicketToDisplay;
            ChangeTitleTypePopupControl.TitleOrTypeChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ChangeAffectsCustomerPopupControl.TicketToDisplay = TicketToDisplay;
            ChangeAffectsCustomerPopupControl.AffectsCustomerChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            AttachmentsControl.TicketToDisplay = TicketToDisplay;
            AttachmentsControl.AttachmentAdded += new TicketPropertyChangedDelegate(TicketPropertyChanged);
            AttachmentsControl.AttachmentRemoved += new TicketAttachmentRemovedDelegate(AttachmentRemoved);
        }

       

        

        private void HookupActionControlEvents()
        {
            ChangePriorityPopupControl.TicketToDisplay = TicketToDisplay;
            ChangePriorityPopupControl.PriorityChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            AssignPopupControl.TicketToDisplay = TicketToDisplay;
            AssignPopupControl.AssignedToChanged += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            TakeOverPopupControl.TicketToDisplay = TicketToDisplay;
            TakeOverPopupControl.TakenOver += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            RequestMoreInfoPopupControl.TicketToDisplay = TicketToDisplay;
            RequestMoreInfoPopupControl.MoreInfoRequested += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            CancelMoreInfoPopupControl.TicketToDisplay = TicketToDisplay;
            CancelMoreInfoPopupControl.MoreInfoCanceled += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            CloseTicketPopupControl.TicketToDisplay = TicketToDisplay;
            CloseTicketPopupControl.TicketClosed += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ReOpenPopupControl.TicketToDisplay = TicketToDisplay;
            ReOpenPopupControl.ReOpened += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            ForceCloseTicketPopupControl.TicketToDisplay = TicketToDisplay;
            ForceCloseTicketPopupControl.TicketForceClosed += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            GiveUpPopupControl.TicketToDisplay = TicketToDisplay;
            GiveUpPopupControl.GivenUp += new TicketPropertyChangedDelegate(TicketPropertyChanged);

            //ResolvePopupControl.TicketToDisplay = TicketToDisplay;
            //ResolvePopupControl.Resolved += new TicketPropertyChangedDelegate(TicketPropertyChanged);

        }

        private bool _enableActionControls = true;
        public bool EnableActionControls
        {
            get
            {
                return _enableActionControls;
            }
            set
            {
                _enableActionControls = value;
            }
        }

        private bool _enableEditControls = true;
        public bool EnableEditControls
        {
            get
            {
                return _enableEditControls;
            }
            set
            {
                _enableEditControls = value;
            }
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
            if(TicketToDisplay != null)
            {
                TicketId.Text = TicketToDisplay.TicketId.ToString();
                TicketTitle.Text = TicketToDisplay.Title;
                if(TicketToDisplay.IsHtml)
                {
                    Details.Text = TicketToDisplay.Details;
                }
                else
                {
                    Details.Text = TicketToDisplay.Details.FormatAsHtml();
                }
                Category.Text = TicketToDisplay.Category;
                //Category.NavigateUrl = string.Format("~/TicketCenter.aspx?View=tagsandcategories&Category={0}", TicketToDisplay.Category);
                TicketType.Text = TicketToDisplay.Type;
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
                //PublishedToKb.Text = (TicketToDisplay.PublishedToKb) ? "Yes" : "No";

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
            if(TicketChanged != null)
            {
                TicketChanged(eventComment);
            }
            AddCommentPanel_CollapsiblePanelExtender.ClientState = "true";
            AddCommentPanel_CollapsiblePanelExtender.Collapsed = true;
           
            DisplayEditControls();
            DisplayActionControls();
            PopulateDisplay();
        }

        void AttachmentRemoved(int fileId, TicketComment eventComment)
        {
            if(TicketAttachmentRemoved != null)
            {
                TicketAttachmentRemoved(fileId, eventComment);
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

        void AddCommentControl_CancelComment(object sender, EventArgs e)
        {
            AddCommentPanel_CollapsiblePanelExtender.ClientState = "true";
            AddCommentPanel_CollapsiblePanelExtender.Collapsed = true;
            AddCommentControl.CheckResolve(false);
        }

        protected void ResolveButton_Click(object sender, EventArgs e)
        {
            if (AddCommentPanel_CollapsiblePanelExtender.Collapsed)
            {
                AddCommentPanel_CollapsiblePanelExtender.ClientState = "false";
                AddCommentPanel_CollapsiblePanelExtender.Collapsed = false;
                AddCommentControl.CheckResolve(true);
            }
            else
            {
                AddCommentPanel_CollapsiblePanelExtender.ClientState = "true";
                AddCommentPanel_CollapsiblePanelExtender.Collapsed = true;
                AddCommentControl.CheckResolve(false);
            }
        }

        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            if (AddCommentPanel_CollapsiblePanelExtender.Collapsed)
            {
               

                AddCommentPanel_CollapsiblePanelExtender.ClientState = "false";
                AddCommentPanel_CollapsiblePanelExtender.Collapsed = false;
                AddCommentControl.CheckResolve(false);
            }
            else
            {
                AddCommentPanel_CollapsiblePanelExtender.ClientState = "true";
                AddCommentPanel_CollapsiblePanelExtender.Collapsed = true;
                AddCommentControl.CheckResolve(false);
            }
        }

    }
}