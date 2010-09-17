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
