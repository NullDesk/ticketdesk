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

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace TicketDesk.Domain.Model
{
    public static class UserSettingExtensions
    {
        /// <summary>
        /// Applies the current sort column settings to an esql object query dynamically.
        /// </summary>
        /// <param name="sorts">The sorts.</param>
        /// <param name="baseQuery">The base object query.</param>
        /// <returns>ObjectQuery{Ticket}.</returns>
        public static ObjectQuery<Ticket> ApplyToQuery(this ICollection<UserTicketListSortColumn> sorts, ObjectQuery<Ticket> baseQuery)
        {
            var sortColumns = sorts.ToList();
            if (sortColumns.Count > 0)
            {
                var skeys = new string[sortColumns.Count];
                for (var i = 0; i < sortColumns.Count; i++)
                {
                    var sortColumn = sortColumns[i];
                    var appd = (sortColumn.SortDirection == ColumnSortDirection.Ascending) ? string.Empty : "DESC";
                    skeys[i] = string.Format("it.{0} {1}", sortColumn.ColumnName, appd);
                }

                var kString = string.Join(",", skeys);
                baseQuery = baseQuery.OrderBy(kString);

            }
            return baseQuery;
        }

        /// <summary>
        /// Applies the current filter settings to an esql object query dynamically.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="baseQuery">The base object query.</param>
        /// <returns>ObjectQuery{Ticket}.</returns>
        public static ObjectQuery<Ticket> ApplyToQuery(this ICollection<UserTicketListFilterColumn> filters, ObjectQuery<Ticket> baseQuery)
        {
            var filterColumns = filters.ToList();
            if (filterColumns.Count > 0)
            {
                var fkeys = new string[filterColumns.Count];
                var fParams = new ObjectParameter[filterColumns.Count];
                for (var i = 0; i < filterColumns.Count; i++)
                {
                    var filterColumn = filterColumns[i];

                    var optr = (filterColumn.UseEqualityComparison.HasValue && !filterColumn.UseEqualityComparison.Value)
                        ? "!="
                        : "=";

                    fkeys[i] = string.Format("it.{0} {1} {2}", filterColumn.ColumnName, optr, "@" + filterColumn.ColumnName);

                    //most of the time esql works with whatever type of param value you pass in, but
                    // enums in our collection are serialized to/from json as integers.
                    // Check if enum, and explicitly convert int value to the correct enum value
                    if (filterColumn.ColumnValueType != null && filterColumn.ColumnValueType.IsEnum)
                    {
                        filterColumn.ColumnValue = Enum.Parse(filterColumn.ColumnValueType, filterColumn.ColumnValue.ToString());
                    }
                    //assigning the type in ctor, then value directly as a param works around issues when the colum val is null.

                    fParams[i] = new ObjectParameter(filterColumn.ColumnName, filterColumn.ColumnValueType)
                    {
                        Value = filterColumn.ColumnValue
                    };

                }

                var wString = string.Join(" and ", fkeys);
                baseQuery = baseQuery.Where(wString, fParams);
            }
            return baseQuery;
        }
    }
}
