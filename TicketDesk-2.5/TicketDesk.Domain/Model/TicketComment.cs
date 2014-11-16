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


        /// <summary>
        /// Creates the activity comment. Infers a comment flag.
        /// </summary>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment content.</param>
        /// <param name="assignedTo">The assigned to.</param>
        /// <param name="args">Optional arguments to use as replacement values in the comment text.</param>
        /// <returns>TicketComment.</returns>
        private static TicketComment CreateActivityComment(
            string commentByUserId,
            TicketActivity activity,
            string comment,
            string assignedTo,
            //string[] notificationSubscribers, 
            params object[] args)
        {
            var cFlag = (string.IsNullOrEmpty(comment)) ? TicketCommentFlag.CommentNotSupplied : TicketCommentFlag.CommentSupplied;
            return CreateActivityComment(commentByUserId, activity, cFlag, comment, assignedTo, args);
        }

        /// <summary>
        /// Creates the activity comment.
        /// </summary>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="commentFlag">The comment flag.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="assignedTo">The assigned to.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>TicketComment.</returns>
        public static TicketComment CreateActivityComment(
            string commentByUserId,
            TicketActivity activity,
            TicketCommentFlag commentFlag,
            string comment,
            string assignedTo,
            //string[] notificationSubscribers,
            params object[] args)
        {
            var c = new TicketComment
            {
                Comment = comment,
                CommentedBy = commentByUserId,
                CommentedDate = DateTime.Now,
                CommentEvent = TicketTextUtility.GetCommentText(activity, commentFlag, args),
                IsHtml = false
            };


            //var isNewOrGiveUp = (assignedTo == null) && (activity == TicketActivity.GiveUp || activity == TicketActivity.Create || activity == TicketActivity.CreateOnBehalfOf);
            //Notification.AddTicketEventNotifications(c, isNewOrGiveUp, notificationSubscribers);

            return c;
        }
    }
}
