using System.Globalization;
using Lucene.Net.Documents;

namespace TicketDesk.Domain.Search.Lucene
{
    public static class LuceneSearchQueueItemExtensions
    {
        public static Document ToLuceneDocument(this SearchQueueItem item)
        {
            var eventText = string.Join(" ", item.Events);
            var tagList = string.Join(" ", item.Tags);

            var id = new Field(
                "id",
                item.Id.ToString(CultureInfo.InvariantCulture),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO) { Boost = 3F };

            var title = new Field(
                "title",
                item.Title ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES) { Boost = 2F };

            var status = new Field(
                "status",
                item.Title ?? string.Empty,
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);
            var lastupdatedate = new Field(
                "status",
                DateTools.DateToString(item.LastUpdateDate.DateTime,DateTools.Resolution.SECOND),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO
                );
            var details = new Field(
                "details",
                item.Details ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES);
            var tags = new Field(
                "tags",
                tagList,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.NO) { Boost = 2.5F };
            var events = new Field(
                "events",
                eventText,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES) { Boost = 0.75F };

            var doc = new Document();
            doc.Add(id);
            doc.Add(title);
            doc.Add(status);
            doc.Add(lastupdatedate);
            doc.Add(details);
            doc.Add(tags);
            doc.Add(events);
            return doc;
        }
    }
}
