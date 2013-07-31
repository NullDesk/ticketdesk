
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TicketDesk.Domain.Model
{

    public class TicketTag
    {
        [DisplayName("Ticket Tag Id")]
        [Required]
        public int TicketTagId { get; set; }

        [DisplayName("Tag Name")]
        [Required]
        [StringLength(100)]
        public string TagName { get; set; }

        [DisplayName("Ticket Id")]
        [Required]
        public int TicketId { get; set; }
    
        public virtual Ticket Ticket { get; set; }

    }
}
