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

using System.Data.Entity;
using System.Threading.Tasks;
using PagedList;
using System;
using System.Linq;
using TicketDesk.Localization.Infrastructure;

public class PagedListAsync<T> : BasePagedList<T>
{
    private PagedListAsync()
    {
    }

    public static async Task<IPagedList<T>> Create(IQueryable<T> superset, int pageNumber, int pageSize)
    {
        var list = new PagedListAsync<T>();
        await list.InitAsync(superset, pageNumber, pageSize);
        return list;
    }

    async Task InitAsync(IQueryable<T> superset, int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException("pageNumber", pageNumber, Strings.PageNumberBelow);
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException("pageSize", pageSize, Strings.PageSizeLess);
        TotalItemCount = superset == null ? 0 : await superset.CountAsync();
        PageSize = pageSize;
        PageNumber = pageNumber;
        PageCount = TotalItemCount > 0 ? (int)Math.Ceiling(TotalItemCount / (double)PageSize) : 0;
        HasPreviousPage = PageNumber > 1;
        HasNextPage = PageNumber < PageCount;
        IsFirstPage = PageNumber == 1;
        IsLastPage = PageNumber >= PageCount;
        FirstItemOnPage = (PageNumber - 1) * PageSize + 1;
        var num = FirstItemOnPage + PageSize - 1;
        LastItemOnPage = num > TotalItemCount ? TotalItemCount : num;
        if (superset == null || TotalItemCount <= 0)
            return;
        Subset.AddRange(pageNumber == 1 ? await superset.Skip(0).Take(pageSize).ToListAsync() : await superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync());
    }
}

public static class PagedListAsyncExtensions
{
    public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> superset, Int32 pageNumber, Int32 pageSize)
    {
        return await PagedListAsync<T>.Create(superset, pageNumber, pageSize);
    }
}