using System;
using System.Collections.Generic;
using TicketDesk.Domain.Localization;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventExtensions
    {
        /// <summary>
        /// Creates an activity event where no priority or user name is applicable to the activity.
        /// </summary>
        /// <param name="set">The set of tickets to which to add this comment.</param>
        /// <param name="eventByUserId">The userId.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment content.</param>
        /// <returns>TicketEvent.</returns>
        public static TicketEvent AddActivityEvent(this ICollection<TicketEvent> set,
            string eventByUserId,
            TicketActivity activity,
            string comment)
        {
            return AddActivityEvent(set, eventByUserId, activity, comment, null, null);
        }

        /// <summary>
        /// Creates the activity comment including a change in priority or a user name is applcable to the comment.
        /// </summary>
        /// <param name="set">The set of tickets to which to add this comment.</param>
        /// <param name="eventByUserId">The userId.</param>
        /// <param name="activity">The activity.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="newPriority">The new priority.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns>TicketEvent.</returns>
        public static TicketEvent AddActivityEvent(this ICollection<TicketEvent> set,
            string eventByUserId,
            TicketActivity activity,
            string comment,
            string newPriority,
            string userName)
        {
            var tc = TicketEvent.CreateActivityEvent(
                 eventByUserId,
                 activity,
                 comment,
                 newPriority,
                 userName
            );
            set.Add(tc);
            return tc;
        }



    }
}
