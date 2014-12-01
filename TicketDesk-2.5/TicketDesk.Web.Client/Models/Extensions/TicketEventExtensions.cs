using TicketDesk.Domain.Model;

namespace TicketDesk.Web.Client.Models
{
    public static class TicketEventExtensions
    {
        public static UserDisplayInfo GetCommentByInfo(this TicketEvent comment)
        {
            return UserDisplayInfo.GetUserInfo(comment.EventBy);
        }
    }
}