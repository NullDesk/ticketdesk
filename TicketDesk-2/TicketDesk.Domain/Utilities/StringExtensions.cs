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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicketDesk.Domain.Utilities
{
    public static class StringExtensions
    {
        public static string ConvertPascalCaseToFriendlyString(this string stringToConvert)
        {
            List<char> cFriendlyName = new List<char>();
            char[] cName = stringToConvert.ToCharArray();
            for (int cIdx = 0; cIdx < cName.Length; cIdx++)
            {
                char c = cName[cIdx];
                if (cIdx > 0 && char.IsUpper(c))
                {
                    cFriendlyName.Add(' ');
                }
                cFriendlyName.Add(c);
            }
            return new string(cFriendlyName.ToArray());
        }
    }
}
