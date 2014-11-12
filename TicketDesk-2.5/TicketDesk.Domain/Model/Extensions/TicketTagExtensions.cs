using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Model
{
    public static class TicketTagExtensions
    {
        public static void AddRange(this ICollection<TicketTag> set, IEnumerable<TicketTag> tags)
        {
            foreach (var tag in tags)
            {
                set.Add(tag);
            }
        }
    }
}
