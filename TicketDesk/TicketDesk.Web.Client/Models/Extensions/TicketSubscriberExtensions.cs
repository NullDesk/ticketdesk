using System.Web.Mvc;
using TicketDesk.Web.Identity;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Domain.Model
{
    public static class TicketSubscriberExtensions
    {


        public static UserDisplayInfo GetSubscriberDisplayInfo(this TicketSubscriber subscriber)
        {
            return GetUserInfo(subscriber.SubscriberId);
        }
        public static UserDisplayInfo GetUserInfo(string userId)
        {
            var userManager = DependencyResolver.Current.GetService<TicketDeskUserManager>();
            return userManager.GetUserInfo(userId);
        }
    }
}