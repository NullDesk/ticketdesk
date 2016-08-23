using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketDesk.Domain.Legacy.Model
{
    public class TicketAttachment
    {
        [DisplayName("Ticket Id")]
        public int? TicketId { get; set; }

        [Key]
        [DisplayName("File Id")]
        [Required]
        public int FileId { get; set; }

        [DisplayName("File Name")]
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [DisplayName("File Size")]
        [Required]
        public int FileSize { get; set; }

        [DisplayName("Is Pending")]
        [DefaultValue(false)]
        public bool IsPending { get; set; }
    


       
    }
}
