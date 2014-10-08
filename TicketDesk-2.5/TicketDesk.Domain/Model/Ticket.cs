namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ticket
    {
        public Ticket()
        {
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketTags = new HashSet<TicketTag>();
        }

        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        public string Details { get; set; }

        public bool IsHtml { get; set; }

        [StringLength(100)]
        public string TagList { get; set; }

        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(100)]
        public string Owner { get; set; }

        [StringLength(100)]
        public string AssignedTo { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrentStatus { get; set; }

        public DateTime CurrentStatusDate { get; set; }

        [Required]
        [StringLength(100)]
        public string CurrentStatusSetBy { get; set; }

        [Required]
        [StringLength(100)]
        public string LastUpdateBy { get; set; }

        public DateTime LastUpdateDate { get; set; }

        [StringLength(25)]
        public string Priority { get; set; }

        public bool AffectsCustomer { get; set; }

        public bool PublishedToKb { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }

        public virtual ICollection<TicketTag> TicketTags { get; set; }
    }
}
