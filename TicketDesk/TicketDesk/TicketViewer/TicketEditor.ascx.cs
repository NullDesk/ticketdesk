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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;
using System.Text;

namespace TicketDesk.TicketViewer
{
    public partial class TicketEditor : System.Web.UI.UserControl
    {
        public event TicketPropertyChangedDelegate TicketEditCompleted;



        private void EditComplete(TicketComment comment)
        {
            if (TicketEditCompleted != null)
            {
                TicketEditCompleted(comment);
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


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (TicketToDisplay != null)
            {
                PopulateDisplay();
            }

        }

        private void PopulateDisplay()
        {
            SetSecurity();

            TicketIdEditLabel.Text = TicketToDisplay.TicketId.ToString();

            TicketTitleEdit.Text = TicketToDisplay.Title;


            DetailsEdit.Value = TicketToDisplay.Details;


            CategoryEdit.SelectedIndex = -1;
            CategoryEdit.Items.Clear();
            foreach (string c in SettingsManager.CategoriesList)
            {
                ListItem catItem = new ListItem(c);
                catItem.Selected = (TicketToDisplay.Category == c);
                CategoryEdit.Items.Add(catItem);
            }


            TicketTypeEdit.SelectedIndex = -1;
            TicketTypeEdit.DataSource = SettingsManager.TicketTypesList;
            TicketTypeEdit.DataBind();
            ListItem item = TicketTypeEdit.Items.FindByValue(TicketToDisplay.Type);
            if (item != null)
            {
                item.Selected = true;
            }

            CreatedByEditLabel.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CreatedBy);

            CreatedDateEditLabel.Text = TicketToDisplay.CreatedDate.ToString("g");

            BuildOwnerUserList();

            AssignedToEditLabel.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.AssignedTo);

            CurrentStatusEditLabel.Text = TicketToDisplay.CurrentStatus;

            CurrentStatusByEditLabel.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.CurrentStatusSetBy);

            CurrentStatusDateEditLabel.Text = TicketToDisplay.CurrentStatusDate.ToString("g");

            LastUpdateByEditLabel.Text = SecurityManager.GetUserDisplayName(TicketToDisplay.LastUpdateBy);

            LastUpdateDateEditLabel.Text = TicketToDisplay.LastUpdateDate.ToString("g");

            BuildPriorityList();

            AffectsCustomerEdit.Checked = TicketToDisplay.AffectsCustomer;

            TagPickerEdit.TagList = TicketToDisplay.TagList;

        }

        private void SetSecurity()
        {
            PriorityEdit.Enabled = SecurityManager.IsStaff;
            CategoryEdit.Enabled = SecurityManager.IsStaff;
            OwnerEdit.Enabled = SecurityManager.IsStaff;
        }

        private void BuildOwnerUserList()
        {
            OwnerEdit.SelectedIndex = -1;
            OwnerEdit.Items.Clear();
            OwnerEdit.Items.Add(new ListItem("-- select --", "-"));
            OwnerEdit.Items.AddRange(GetOwnerUserList());
        }


        public ListItem[] GetOwnerUserList()
        {
            List<ListItem> returnUsers = new List<ListItem>();
            User[] users = SecurityManager.GetTicketSubmitterUsers();
            foreach (User user in users)
            {
                ListItem ownerItem = new ListItem(user.DisplayName, user.Name);
                ownerItem.Selected = (user.Name.ToUpperInvariant() == TicketToDisplay.Owner.ToUpperInvariant());
                returnUsers.Add(ownerItem);

            }
            return returnUsers.ToArray();
        }



        private void BuildPriorityList()
        {
            PriorityEdit.SelectedIndex = -1;
            PriorityEdit.Items.Clear();
            if (string.IsNullOrEmpty(TicketToDisplay.Priority))
            {
                PriorityEdit.Items.Add(string.Empty);
            }
            string[] priorities = SettingsManager.PrioritiesList;
            foreach (string p in priorities)
            {
                ListItem priItem = new ListItem(p);
                priItem.Selected = (TicketToDisplay.Priority == p);
                PriorityEdit.Items.Add(priItem);
            }
        }

        public bool Save(TicketComment eventComment)
        {
            Dictionary<string, KeyValuePair<string, string>> fieldEventTextList = new Dictionary<string, KeyValuePair<string, string>>();//key is simple event test, value is full event text;

            if (TicketTitleEdit.Text != TicketToDisplay.Title)
            {
                fieldEventTextList.Add("changed ticket's title", GetFieldEventValues(TicketToDisplay.Title, TicketTitleEdit.Text));
                TicketToDisplay.Title = TicketTitleEdit.Text;
            }
            if (DetailsEdit.Value != TicketToDisplay.Details)
            {
                fieldEventTextList.Add("changed ticket's details", GetFieldEventValues(string.Empty,string.Empty));
                TicketToDisplay.IsHtml = true;
                TicketToDisplay.Details = DetailsEdit.Value;
            }
            if (CategoryEdit.Enabled && CategoryEdit.SelectedValue != TicketToDisplay.Category)
            {
                fieldEventTextList.Add("changed ticket's category", GetFieldEventValues(TicketToDisplay.Category, CategoryEdit.SelectedValue));
                TicketToDisplay.Category = CategoryEdit.SelectedValue;
            }
            if (PriorityEdit.Enabled && PriorityEdit.SelectedValue != TicketToDisplay.Priority && !string.IsNullOrEmpty(PriorityEdit.SelectedValue))
            {
                fieldEventTextList.Add("changed ticket's priority", GetFieldEventValues(TicketToDisplay.Priority, PriorityEdit.SelectedValue));
                TicketToDisplay.Priority = PriorityEdit.SelectedValue;
            }
            if (OwnerEdit.Enabled && OwnerEdit.SelectedValue != TicketToDisplay.Owner)
            {
                fieldEventTextList.Add("changed ticket's owner", GetFieldEventValues(TicketToDisplay.Owner, OwnerEdit.SelectedValue));
                TicketToDisplay.Owner = OwnerEdit.SelectedValue;
            }
            if (AffectsCustomerEdit.Checked != TicketToDisplay.AffectsCustomer)
            {
                KeyValuePair<string,string> e = GetFieldEventValues(((TicketToDisplay.AffectsCustomer) ? "Yes" : "No"), ((AffectsCustomerEdit.Checked) ? "Yes" : "No"));
                fieldEventTextList.Add("changed ticket's affects customer flag", e);
                TicketToDisplay.AffectsCustomer = AffectsCustomerEdit.Checked;
            }

            string[] tags = TagManager.GetTagsFromString(TagPickerEdit.TagList);
            string newTagList = TicketToDisplay.TagList = string.Join(",", tags);
            if (newTagList != TicketToDisplay.TagList)
            {
                fieldEventTextList.Add("changed ticket's tags", GetFieldEventValues(TicketToDisplay.TagList, newTagList));
                TicketToDisplay.TicketTags.Clear();
                foreach (string tag in tags)
                {
                    TicketTag tTag = new TicketTag();
                    tTag.TagName = tag;
                    TicketToDisplay.TicketTags.Add(tTag);
                }
                TicketToDisplay.TagList = newTagList;
            }

            if (fieldEventTextList.Count > 0)
            {
                if (fieldEventTextList.Count > 1)
                {
                    eventComment.CommentEvent = "has edited multiple fields";
                }
                else
                {
                    foreach (var v in fieldEventTextList)//lazy, using foreach even though only one element... just a more convienient way to get at the content
                    {
                        if (v.Key == "changed ticket's details")
                        {
                            eventComment.CommentEvent = v.Key;
                        }
                        else
                        {
                            eventComment.CommentEvent = string.Format("{0} from {1} to {2}", v.Key, v.Value.Key, v.Value.Value);
                        }
                    }
                }

                if (string.IsNullOrEmpty(eventComment.Comment))
                {
                    eventComment.CommentEvent = eventComment.CommentEvent + " without comment";
                }

                StringBuilder sb = new StringBuilder();
                foreach (var v in fieldEventTextList)
                {
                    sb.Append("<div class='MultiFieldEditContainer'>");
                    sb.Append("<div class='MiltiFieldEditFieldName'>");
                    string fromtoString = GetFieldEventValuesHtml(v.Value.Key, v.Value.Value);
                    sb.Append(v.Key.Substring(0, 1).ToUpper() + v.Key.Substring(1));
                    sb.Append("</div>");
                    sb.Append(fromtoString);
                    sb.Append("</div>");
                }
                if (!string.IsNullOrEmpty(eventComment.Comment))
                {
                    sb.Append("<hr />");
                    sb.Append(eventComment.Comment);
                }
                eventComment.Comment = sb.ToString();

                EditComplete(eventComment);
                return true;
            }
            else
            {
                return false;
            }


        }

        private string GetFieldEventValuesHtml(string from, string to)
        {
            return string.Format("<div class='MultiFieldEditFactsContainer'><div class='MultiFieldEditOldValue'><label>From:&nbsp;&nbsp;</label>{0}</div><div class='MultiFieldEditNewValue'><label>To:&nbsp;&nbsp;</label>{1}</div></div>", from, to);

        }


       
        private KeyValuePair<string, string> GetFieldEventValues(string from, string to)
        {
            return new KeyValuePair<string, string>(from, to);
        }

    }
}