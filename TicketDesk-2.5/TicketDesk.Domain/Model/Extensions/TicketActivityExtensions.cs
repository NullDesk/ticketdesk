using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Infrastructure;

namespace TicketDesk.Domain.Model
{
    public static class TicketActivityExtensions
    {
        public static bool CommentRequired(this TicketActivity enumeration)
        {
            var req = false;
            var type = enumeration.GetType();
            var memberInfo = type.GetMember(enumeration.ToString());

            //if there is member information
            if (memberInfo.Length > 0)
            {
                req = memberInfo[0].GetCustomAttributes(typeof(CommentRequiredAttribute), false).Any();
            }

            return req;
        }
    }
}
