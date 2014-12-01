using System;

namespace TicketDesk.Domain.Search
{
    public class SearchQueueItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string Status { get; set; }
        public DateTimeOffset LastUpdateDate { get; set; }
        public string[] Tags { get; set; }
        public string[] Events { get; set; }
    }
}


