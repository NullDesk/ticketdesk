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
