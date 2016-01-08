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
using RedDog.Search.Model;
using TicketDesk.Search.Common;

namespace TicketDesk.Search.Azure
{
    public static class AzureSearchIndexItemExtensions
    {
        public static IndexOperation ToIndexOperation(this SearchIndexItem item)
        {
            var op = new IndexOperation(IndexOperationType.Upload, "id", item.Id.ToString(CultureInfo.InvariantCulture))
                .WithProperty("title", item.Title)
                .WithProperty("projectid", item.ProjectId.ToString(CultureInfo.InvariantCulture))
                .WithProperty("status", item.Status)
                .WithProperty("lastupdatedate", item.LastUpdateDate)
                .WithProperty("details", item.Details)
                .WithProperty("tags", item.Tags)
                .WithProperty("events", item.Events);
            return op;
        }
    }
}
