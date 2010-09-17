using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcPaging
{
    public static class PageLinqExtensions
    {
        public static PageOfList<T> ToPageOfList<T>
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            var itemIndex = pageIndex * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize);
            var totalItemCount = allItems.Count();
            return new PageOfList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);

        }
    }
}
