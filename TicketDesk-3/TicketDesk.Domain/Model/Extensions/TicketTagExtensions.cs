using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Model
{

    public static class TicketTagExtensions
    {
        public static IEnumerable<TicketTag> GetTagList(this DbSet<TicketTag> ticketTags)
        {

            return from t in ticketTags
                group t by t.TagName
                into g
                select g.FirstOrDefault();


        }

        
    }
}