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
using TicketDesk.Engine;

namespace TicketDesk.Controls
{
    public partial class TagPicker : System.Web.UI.UserControl
    {
        public string TagList
        {
            get
            {
                return TagsTextBox.Text;
            }
            set
            {
                TagsTextBox.Text = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            TagsPanel.Visible = (SecurityManager.IsStaff || SecurityManager.SubmitterCanEditTags);
        }
    }
}