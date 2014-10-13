using System.ComponentModel;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketComment
    {
        public TicketComment()
        {
            TicketEventNotifications = new HashSet<TicketEventNotification>();
        }

        [Key]
        [Column(Order = 0)]
        [DisplayName("Ticket Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DisplayName("Comment Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [StringLength(500)]
        [DisplayName("Comment Event")]
        public string CommentEvent { get; set; }

        [Column(TypeName = "ntext")]
        [DisplayName("Comment")]
        public string Comment { get; set; }

        [DisplayName("Is Html")]
        public bool IsHtml { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Commented By")]
        public string CommentedBy { get; set; }

        [Required]
        [DisplayName("Commented Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset CommentedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [DisplayName("Version")]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ICollection<TicketEventNotification> TicketEventNotifications { get; set; }
    }
}
