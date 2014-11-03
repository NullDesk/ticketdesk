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

namespace TicketDesk.Web.Client.Helpers
{
    public class PagerOptions
    {
        public PagerOptions()
        {
            this.IndexParameterName = "id";
            this.MaximumPageNumbers = 5;
            this.PageNumberFormatString = "{0}";
            this.SelectedPageNumberFormatString = "{0}";
            this.ShowPrevious = true;
            this.PreviousText = "<";
            this.ShowNext = true;
            this.NextText = ">";
            this.ShowNumbers = true;
        }

        public string IndexParameterName { get; set; }
        public string PageNumberFormatString { get; set; }
        public string SelectedPageNumberFormatString { get; set; }
        public object LinkAttributes { get; set; }
        public int MaximumPageNumbers { get; set; }
        public bool ShowPrevious { get; set; }
        public string PreviousText { get; set; }
        public bool ShowNext { get; set; }
        public string NextText { get; set; }
        public bool ShowNumbers { get; set; }

    }
}