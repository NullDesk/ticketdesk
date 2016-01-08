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

using System.Collections.Generic;

namespace TicketDesk.Domain
{
    public static class StringExtensions
    {
        public static string ConvertPascalCaseToFriendlyString(this string stringToConvert)
        {
            var cFriendlyName = new List<char>();
            var cName = stringToConvert.ToCharArray();
            for (var cIdx = 0; cIdx < cName.Length; cIdx++)
            {
                var c = cName[cIdx];
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
