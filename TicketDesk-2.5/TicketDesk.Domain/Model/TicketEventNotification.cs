namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketEventNotification
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CommentId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string NotifyUser { get; set; }

        [Required]
        [StringLength(100)]
        public string NotifyUserDisplayName { get; set; }

        [Required]
        [StringLength(255)]
        public string NotifyEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string NotifyUserReason { get; set; }

        public DateTime CreatedDate { get; set; }

        public int DeliveryAttempts { get; set; }

        public DateTime? LastDeliveryAttemptDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime? NextDeliveryAttemptDate { get; set; }

        [Required]
        [StringLength(100)]
        public string EventGeneratedByUser { get; set; }

        public virtual TicketComment TicketComment { get; set; }
    }
}
