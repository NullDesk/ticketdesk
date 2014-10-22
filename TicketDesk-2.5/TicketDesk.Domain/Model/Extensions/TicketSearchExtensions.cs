using System.Globalization;
using System.Linq;
using Lucene.Net.Documents;

namespace TicketDesk.Domain.Model.Extensions
{
    public static class TicketSearchExtensions
    {

        public static Document CreateSearchDocument(this Ticket ticket)
        {
            var commentText = string.Join(" ", ticket.TicketComments.Select(c => c.Comment));

            var id = new Field(
                "id",
                ticket.TicketId.ToString(CultureInfo.InvariantCulture),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO) { Boost = 0.8F };

            var title = new Field(
                "title",
                ticket.Title ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES) { Boost = 1.5F };


            var details = new Field(
                "details",
                ticket.Details ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES) { Boost = 1F };

            var tags = new Field(
                "tags",
                ticket.TagList ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.NO) { Boost = 3F };

            var comments = new Field(
                "comments",
                commentText ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES) { Boost = 0.5F };

            var doc = new Document();
            doc.Add(id);
            doc.Add(title);
            doc.Add(details);
            doc.Add(tags);
            doc.Add(comments);
            return doc;

        }
        
        
    }
}
