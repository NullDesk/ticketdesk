using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.Domain.Search
{
    public class SearchResultItem
    {
        public int Id { get; set; }
        public float SearchScore { get; set; }
    }
}
