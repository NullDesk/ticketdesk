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
using System.Text;

namespace TicketDesk.Web.Client.Helpers
{
    public class PagerItem
    {
        public PagerItem(string text, string url, bool isSelected, int pageIndex)
        {
            this.Text = text;
            this.Url = url;
            this.IsSelected = isSelected;
            this.PageIndex = pageIndex;
        }

        public string Text { get; set; }
        public string Url { get; set; }
        public bool IsSelected { get; set; }
        public int PageIndex { get; set; }
    }
}
