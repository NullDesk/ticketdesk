using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Localization;

namespace TicketDesk.Domain.Model.Extensions
{
    public static class TicketCommentExtensions
    {
        /// <summary>
        /// Creates an activity comment where no priority or user name is applicable to the activity.
        /// </summary>
        /// <param name="set">The set of tickets to which to add this comment.</param>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment content.</param>
        /// <returns>TicketComment.</returns>
        public static TicketComment AddActivityComment(this ICollection<TicketComment> set,
            string commentByUserId,
            TicketActivity activity,
            string comment)
        {

            //TODO: keep this here, or move to extension methods on TicketComment dbset?
            return AddActivityComment(set, commentByUserId, activity, comment, null, null);
        }

        /// <summary>
        /// Creates the activity comment including a change in priority or a user name is applcable to the comment.
        /// </summary>
        /// <param name="set">The set of tickets to which to add this comment.</param>
        /// <param name="commentByUserId">The comment by user identifier.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="newPriority">The new priority.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>TicketComment.</returns>
        public static TicketComment AddActivityComment(this ICollection<TicketComment> set,
            string commentByUserId,
            TicketActivity activity,
            string comment,
            string newPriority,
            string userName)
        {
            //TODO: keep this here, or move to extension methods on TicketComment dbset?
            var tc = new TicketComment
            {
                Comment = comment,
                CommentedBy = commentByUserId,
                CommentedDate = DateTime.Now,
                CommentEvent = TicketTextUtility.GetCommentEventText(activity, newPriority, userName),
                IsHtml = false
            };
            set.Add(tc);

            return tc;
        }
    }
}
