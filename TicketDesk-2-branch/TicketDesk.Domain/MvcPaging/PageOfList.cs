using System;
using System.Collections.Generic;

namespace MvcPaging
{
    public class PageOfList<T> : List<T>, IPageOfList<T>
    {
        public PageOfList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
        {
            this.AddRange(items);
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalItemCount = totalItemCount;
            this.TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; private set; }

    }
}
