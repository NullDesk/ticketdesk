// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.Linq;
using TicketDesk.Domain.Infrastructure;

namespace TicketDesk.Domain.Model
{
    public static class TicketActivityExtensions
    {
        public static bool IsCommentRequired(this TicketActivity enumeration)
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
