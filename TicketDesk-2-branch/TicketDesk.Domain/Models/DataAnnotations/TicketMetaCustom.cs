using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data.Objects.DataClasses;

namespace TicketDesk.Domain.Models.DataAnnotations
{
    public partial class TicketMeta
    {
        [DisplayName("Attachments")]
        public EntityCollection<TicketAttachment> TicketAttachments { get; set; }
    }
}
