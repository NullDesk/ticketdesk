// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketDesk.Domain.Model
{
 

    public class TicketEventNotification
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

        public DateTimeOffset CreatedDate { get; set; }

        public int DeliveryAttempts { get; set; }

        public DateTimeOffset? LastDeliveryAttemptDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTimeOffset? NextDeliveryAttemptDate { get; set; }

        [Required]
        [StringLength(100)]
        public string EventGeneratedByUser { get; set; }

        public virtual TicketEvent TicketEvent { get; set; }
    }
}
