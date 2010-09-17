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
            this.Visible = (AttachmentsRepeater.Items.Count > 0);
        }

        public string GetAttachmentLinkUrl(int fileId)
        {
            return string.Format("~/DownloadAttachment.ashx?fileId={0}", fileId.ToString());
        }



        internal void Refresh()
        {
            AttachmentsRepeater.DataBind();
        }
    }
}