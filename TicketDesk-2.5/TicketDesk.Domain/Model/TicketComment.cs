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
using System.ComponentModel;
using TicketDesk.Domain.Localization;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TicketComment
    {
        public TicketComment()
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            TicketEventNotifications = new HashSet<TicketEventNotification>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [StringLength(500)]
        public string CommentEvent { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        public bool IsHtml { get; set; }

        [Required]
        [StringLength(100)]
        public string CommentedBy { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset CommentedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ICollection<TicketEventNotification> TicketEventNotifications { get; set; }


        /// <summary>
        /// Creates an activity comment where no priority or user name is applicable to the activity.
        /// </summary>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment content.</param>
        /// <returns>TicketComment.</returns>
        public static TicketComment CreateActivityComment(
            string commentByUserId,
            TicketActivity activity,
            string comment)
        {
            return CreateActivityComment(commentByUserId, activity, comment, null, null);
        }

        /// <summary>
        /// Creates the activity comment including a change in priority or a user name is applcable to the comment.
        /// </summary>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="newPriority">The new priority.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>TicketComment.</returns>
        public static TicketComment CreateActivityComment(
            string commentByUserId,
            TicketActivity activity,
            string comment,
            string newPriority,
            string userName)
        {
            var tc = new TicketComment
            {
                Comment = comment,
                CommentedBy = commentByUserId,
                CommentedDate = DateTime.Now,
                CommentEvent = TicketTextUtility.GetCommentEventText(activity, newPriority, userName),
                IsHtml = false
            };

            return tc;
        }
    }
}
