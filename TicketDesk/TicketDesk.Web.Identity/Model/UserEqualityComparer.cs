using System.Collections.Generic;
using TicketDesk.Web.Identity.Model;

namespace TicketDesk.Web.Identity.Model
{
    public class StandardUserEqualityComparer : IEqualityComparer<TicketDeskUser>
    {
        public bool Equals(TicketDeskUser x, TicketDeskUser y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(TicketDeskUser obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class UniqueNameEmailDisplayUserEqualityComparer : IEqualityComparer<TicketDeskUser>
    {
        public bool Equals(TicketDeskUser x, TicketDeskUser y)
        {
            return x.DisplayName == y.DisplayName && x.Email == y.Email;
        }

        public int GetHashCode(TicketDeskUser obj)
        {
            return obj.DisplayName.GetHashCode() + obj.Email.GetHashCode();
        }
    }

}