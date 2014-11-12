// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    public static class UserSettingExtensions
    {
        /// <summary>
        /// Gets the user list setting by name.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="listName">Name of the list.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>UserTicketListSetting.</returns>
        public static UserTicketListSetting GetUserListSettingByName(this DbSet<UserSetting> settings, string listName, string userId)
        {
            return GetUserSetting(settings, userId).GetUserListSettingByName(listName);
            //return GetUserSettings(settings, userId).ListSettings.FirstOrDefault(us => us.ListName == listName);
        }


        /// <summary>
        /// Gets all user list settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>ICollection{UserTicketListSetting}.</returns>
        public static ICollection<UserTicketListSetting> GetUserListSettings(this DbSet<UserSetting> settings,
            string userId)
        {
            return GetUserSetting(settings, userId).ListSettings;

        }

        /// <summary>
        /// Gets the user settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>UserSetting.</returns>
        public static UserSetting GetUserSetting(this DbSet<UserSetting> settings, string userId)
        {
            return (settings.FirstOrDefault(us => us.UserId == userId) ??
                    UserSetting.GetDefaultSettingsForUser(userId));
        }

        /// <summary>
        /// Applies the current sort column settings to an esql object query dynamically.
        /// </summary>
        /// <param name="sorts">The sorts.</param>
        /// <param name="baseQuery">The base object query.</param>
        /// <returns>ObjectQuery{Ticket}.</returns>
        public static ObjectQuery<Ticket> ApplyToQuery(this ICollection<UserTicketListSortColumn> sorts, ObjectQuery<Ticket> baseQuery)
        {
            var sortColumns = sorts.ToList();
            string kString = null;
            if (sortColumns.Count > 0)
            {
                var skeys = new string[sortColumns.Count];
                for (var i = 0; i < sortColumns.Count; i++)
                {
                    var sortColumn = sortColumns[i];
                    var appd = (sortColumn.SortDirection == ColumnSortDirection.Ascending) ? string.Empty : "DESC";
                    skeys[i] = string.Format("it.{0} {1}", sortColumn.ColumnName, appd);
                }

                kString = string.Join(",", skeys);
                baseQuery = baseQuery.OrderBy(kString ?? "it.TicketId DESC");

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
            string wString = null;
            if (filterColumns.Count > 0)
            {
                var fkeys = new string[filterColumns.Count];
                var fParams = new ObjectParameter[filterColumns.Count];
                for (var i = 0; i < filterColumns.Count; i++)
                {
                    var filterColumn = filterColumns[i];

                    var optr = (filterColumn.UseEqualityComparison.HasValue && !filterColumn.UseEqualityComparison.Value) ? "!=" : "=";

                    fkeys[i] = string.Format("it.{0} {1} {2}", filterColumn.ColumnName, optr, "@" + filterColumn.ColumnName);


                    //most of the time esql works with whatever type of param value you pass in, but
                    // enums in our collection are serialized to/from json as integers.
                    // Check if enum, and explicitly convert int value to the correct enum value
                    if (filterColumn.ColumnValueType.IsEnum)
                    {
                        filterColumn.ColumnValue = Enum.Parse(filterColumn.ColumnValueType, filterColumn.ColumnValue.ToString());
                    }

                    //assigning the type in ctor, then value directly as a param works around issues when the colum val is null.
                    fParams[i] = new ObjectParameter(filterColumn.ColumnName, filterColumn.ColumnValueType)
                    {
                        Value = filterColumn.ColumnValue
                    };
                }

                wString = string.Join(" and ", fkeys);
                baseQuery = baseQuery.Where(wString, fParams);
            }
            return baseQuery;
        }
    }
}
