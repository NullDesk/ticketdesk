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

using System.Globalization;
using System.Linq;
using Lucene.Net.Documents;
using TicketDesk.Search.Common;

namespace TicketDesk.Search.Lucene
{
    public static class LuceneSearchIndexItemExtensions
    {
        public static Document ToLuceneDocument(this SearchIndexItem item)
        {
            var eventText = item.Events != null && item.Events.Any() ? string.Join(" ", item.Events) : string.Empty;
            var tagList = item.Tags != null && item.Tags.Any() ? string.Join(" ", item.Tags) : string.Empty;

            var id = new Field(
                "id",
                item.Id.ToString(CultureInfo.InvariantCulture),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO)
            { Boost = 3F };

            var projectid = new Field(
                "projectid",
                item.ProjectId.ToString(CultureInfo.InvariantCulture),
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO)
            { Boost = 0F };

            var title = new Field(
                "title",
                item.Title ?? string.Empty,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES)
            { Boost = 2F };

            var status = new Field(
                "status",
                item.Title ?? string.Empty,
                Field.Store.YES,
                Field.Index.NOT_ANALYZED,
                Field.TermVector.NO);
            var lastupdatedate = new Field(
                "status",
                DateTools.DateToString(item.LastUpdateDate.DateTime, DateTools.Resolution.SECOND),
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
                Field.TermVector.NO)
            { Boost = 2.5F };
            var events = new Field(
                "events",
                eventText,
                Field.Store.NO,
                Field.Index.ANALYZED,
                Field.TermVector.YES)
            { Boost = 0.75F };

            var doc = new Document();
            doc.Add(id);
            doc.Add(projectid);
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
