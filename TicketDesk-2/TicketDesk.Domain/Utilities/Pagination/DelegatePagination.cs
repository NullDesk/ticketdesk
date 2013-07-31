using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TicketDesk.Domain.Utilities.Pagination
{
	/// <summary>
	/// Event arguments passed to the Delegate
	/// </summary>
	public class PagingQueryEventArgs : EventArgs
	{
		/// <summary>
		/// Page number requested
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		/// Page size requested
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		/// Any additional arguments passed to the method that executes the query
		/// </summary>
		public object AdditionalArgs { get; set; }

		/// <summary>
		/// Return value of total items matching the query
		/// </summary>
		public int TotalItems { get; set; }
	}

	/// <summary>
	/// Delegate method invoked to
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="sender"></param>
	/// <param name="e">Event arguments passed to the method</param>
	/// <returns>The page of data based on the parameters</returns>
	public delegate IList<T> DoQueryDelegate<T>(object sender, PagingQueryEventArgs e);

	/// <summary>
	/// Executes an delegate method in order to created a paged set of objects.
	/// The query is not executed until the DelegatePagination is enumerated or one of its properties is invoked.
	/// </summary>
	/// <typeparam name="T">Type of objects in the collection.</typeparam>
	public class DelegatePagination<T> : IPagination<T>
	{
		private IList<T> _results;
		private object _additionalArgs;
		private bool _doRefresh = true;
		private int _totalItems;
		private int _pageNumber;
		private int _pageSize;

		/// <summary>
		/// Event raised when query is performed.
		/// </summary>
		public event DoQueryDelegate<T> DoQuery;

		/// <summary>
		/// Creates a new instance of the <see cref="DelegatePagination{T}"/> class.
		/// </summary>
		/// <param name="method">The method invoked to fetch data.</param>
		/// <param name="pageNumber">The current page number.</param>
		/// <param name="pageSize">Number of items per page.</param>
		/// <param name="additionalArgs">Additional data passed in the AdditionalArgs</param>
		public DelegatePagination(DoQueryDelegate<T> method, int pageNumber, int pageSize, object additionalArgs)
		{
			DoQuery = method;
			_pageNumber = pageNumber;
			_pageSize = pageSize;
			_additionalArgs = additionalArgs;
		}

		/// <summary>
		/// Raises the event to execute the query.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDoQuery(PagingQueryEventArgs e)
		{
			if(DoQuery != null)
			{
				_results = DoQuery(this, e).ToList();
				TotalItems = e.TotalItems;
				_doRefresh = false;
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			TryExecuteQuery();
			return _results.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected void TryExecuteQuery()
		{
			if(_doRefresh)
			{
				OnDoQuery(new PagingQueryEventArgs {PageNumber = PageNumber, PageSize = PageSize, AdditionalArgs = _additionalArgs});
			}
		}

		public int PageNumber
		{
			get { return _pageNumber; }
			set
			{
				if(value != _pageNumber)
				{
					_doRefresh = true;
					_pageNumber = value;
				}
			}
		}

		public int PageSize
		{
			get { return _pageSize; }
			set
			{
				if(value != _pageSize)
				{
					_doRefresh = true;
					_pageSize = value;
				}
			}
		}

		public int TotalItems
		{
			get { return _totalItems; }
			private set
			{
				if(value != _totalItems)
				{
					_doRefresh = true;
					_totalItems = value;
				}
			}
		}

		public object AdditionalArgs
		{
			get { return _additionalArgs; }
			set
			{
				if(value != _additionalArgs)
				{
					_doRefresh = true;
					_additionalArgs = value;
				}
			}
		}

		public int TotalPages
		{
			get
			{
				TryExecuteQuery();
				return (int)Math.Ceiling(((double)TotalItems) / PageSize);
			}
		}

		public int FirstItem
		{
			get
			{
				TryExecuteQuery();
				return ((PageNumber - 1) * PageSize) + 1;
			}
		}

		public int LastItem
		{
			get
			{
				TryExecuteQuery();
				return FirstItem + _results.Count - 1;
			}
		}

		public bool HasPreviousPage
		{
			get
			{
				TryExecuteQuery();
				return PageNumber > 1;
			}
		}

		public bool HasNextPage
		{
			get
			{
				TryExecuteQuery();
				return PageNumber < TotalPages;
			}
		}
	}
}