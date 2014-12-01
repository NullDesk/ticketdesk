using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public static class TicketCommentExtensions
    {
        public static UserDisplayInfo GetCommentByInfo(this TicketComment comment)
        {
            return UserDisplayInfo.GetUserInfo(comment.CommentedBy);
        }
    }
}