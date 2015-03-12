using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Model
{
    public class TicketSubscriber
    {
        [Key]
        [Column(Order = 0)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string Subscriber { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
