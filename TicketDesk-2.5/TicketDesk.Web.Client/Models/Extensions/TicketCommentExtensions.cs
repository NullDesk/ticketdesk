using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models.Extensions
{
    public static class TicketCommentExtensions
    {
        public static UserDisplayInfo GetCommentByInfo(this TicketComment comment)
        {
            return UserDisplayInfo.GetUserInfo(comment.CommentedBy);
        }
    }
}