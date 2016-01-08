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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicketDesk.Domain;
using TicketDesk.Localization.Models;

namespace TicketDesk.Web.Client
{
    /// <summary>
    /// This class contains a series of helpers and extensions for transforming collections to MVC select lists.
    /// </summary>
    public static class SelectListExtensions
    {
        private static string DefaultItemText { get { return Strings.SelectList_DefaultItem; } }

        #region general select list

        public static MultiSelectList ToMultiSelectList<TSource, TValue>
        (
            this IEnumerable<TSource> items,
            Func<TSource, TValue> valueField,
            Func<TSource, string> textField,
            ICollection selectedValues = null,
            bool includeDefaultItem = false
        ) where TValue : class
        {
            var list = items.Select(i => new SelectListQueryItem<TValue> { Id = valueField(i), Name = textField(i) }).ToList();
            AddDefaultItem(list, includeDefaultItem);
            return new MultiSelectList(list, "ID", "Name", selectedValues);
        }

        public static MultiSelectList ToMultiSelectList<TSource, TValue>
       (
           this IEnumerable<TSource> items,
           Func<TSource, TValue?> valueField,
           Func<TSource, string> textField,
           ICollection selectedValues = null,
           bool includeDefaultItem = false
       ) where TValue : struct
        {
            var list = items.Select(i => new SelectListQueryItem<TValue?> { Id = valueField(i), Name = textField(i) }).ToList();
            AddDefaultItem(list, includeDefaultItem);
            return new MultiSelectList(list, "ID", "Name", selectedValues);
        }

        public static SelectList ToSelectList<TSource, TValue>
        (
            this IEnumerable<TSource> items,
            Func<TSource, TValue> valueField,
            Func<TSource, string> textField,
            object selectedValue = null,
            bool includeDefaultItem = true
        ) where TValue : class
        {
            var list = items.Select(i => new SelectListQueryItem<TValue> { Id = valueField(i), Name = textField(i) }).ToList();
            AddDefaultItem(list, includeDefaultItem);
            return new SelectList(list, "ID", "Name", selectedValue);
        }

        public static SelectList ToSelectList<TSource, TValue>
       (
           this IEnumerable<TSource> items,
           Func<TSource, TValue?> valueField,
           Func<TSource, string> textField,
           object selectedValue = null,
           bool includeDefaultItem = true
       ) where TValue : struct
        {
            var list = items.Select(i => new SelectListQueryItem<TValue?> { Id = valueField(i), Name = textField(i) }).ToList();
            AddDefaultItem(list, includeDefaultItem);
            return new SelectList(list, "ID", "Name", selectedValue);
        }

        public static SelectList ToSelectList<TSource, TValue>
         (
             this IEnumerable<TSource> items,
             Func<TSource, TValue> valueField,
             Func<TSource, string> textField,
             string emptyText,
             TValue emptyValue,
             object selectedValue = null

         ) where TValue : class
        {
            var list = items.Select(i => new SelectListQueryItem<TValue> { Id = valueField(i), Name = textField(i) }).ToList();
            list.Insert(0, new SelectListQueryItem<TValue> { Name = emptyText, Id = emptyValue });
            return new SelectList(list, "ID", "Name", selectedValue);
        }

        #endregion

        #region empty / default select lists

        public static SelectList GetEmptySelectList<T>(bool includeDefaultItem = true)
        {
            if (includeDefaultItem)
            {
                var items = new List<SelectListQueryItem<T>>();
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                AddDefaultItem(items, includeDefaultItem);
                return new SelectList(items, "ID", "Name");
            }
            return null;
        }

        private static SelectListQueryItem<T> GetDefaultSelectListItem<T>()
        {
            return new SelectListQueryItem<T> { Name = DefaultItemText, Id = default(T) };
        }

        #endregion

        #region select list for enum

        /// <summary>
        /// Gets a select list from an enumeration.
        /// </summary>
        /// <remarks>
        /// If a DisplayAttribute is applied to the items within the enum then
        /// this method will use the display name for the text property in the SelectList
        /// </remarks>
        /// <example>
        /// var sList = new MyEnum().ToSelectList()
        /// </example>
        /// <param name="enumeration"></param>
        /// <param name="selectedValue"></param>
        /// <param name="includeDefaultItem"></param>
        /// <returns></returns>
        public static SelectList ToSelectList(this Enum enumeration, object selectedValue = null, bool includeDefaultItem = true)
        {
            var list = from Enum e in Enum.GetValues(enumeration.GetType())
                       select new
                       {
                           // ReSharper disable once AssignNullToNotNullAttribute
                           ID = Enum.Parse(enumeration.GetType(), Enum.GetName(enumeration.GetType(), e)),
                           Name = e.GetDisplayName()
                       };
            return list.ToSelectList(i => i.ID, i => i.Name, selectedValue, includeDefaultItem);
        }

        public static MultiSelectList ToMultiSelectList(this Enum enumeration, ICollection selectedValues = null, bool includeDefaultItem = false)
        {
            var list = from Enum e in Enum.GetValues(enumeration.GetType())
                       select new
                       {
                           // ReSharper disable once AssignNullToNotNullAttribute
                           ID = Enum.Parse(enumeration.GetType(), Enum.GetName(enumeration.GetType(), e)),
                           Name = e.GetDisplayName()
                       };
            return list.ToSelectList(i => i.ID, i => i.Name, selectedValues, includeDefaultItem);
        }


        #endregion

        #region utility

        internal class SelectListQueryItem<T>
        {
            public string Name { get; set; }
            public T Id { get; set; }
        }

        private static void AddDefaultItem<T>(IList<SelectListQueryItem<T>> list, bool includeDefaultItem)
        {
            if (includeDefaultItem)
            {
                list.Insert(0, GetDefaultSelectListItem<T>());
            }
        }

        #endregion



    }
}