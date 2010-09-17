using System.Collections.Generic;

namespace MvcPaging
{

    public interface IPageOfList<T> : IList<T>
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalPageCount { get; }
        int TotalItemCount { get; }
    }
}