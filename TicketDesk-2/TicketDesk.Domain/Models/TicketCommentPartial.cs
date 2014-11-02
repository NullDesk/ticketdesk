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
using TicketDesk.Domain.Utilities;

namespace TicketDesk.Domain.Models
{
    public partial class TicketComment
    {
        public string HtmlComment
        {
            get
            {
                var md = new Markdown();
                return (this.IsHtml) ? this.Comment : md.Transform(this.Comment, true);
            }
        }
    }
}
