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
using System.Linq;
using System.Web;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Hosting;
using System.Linq.Expressions;

namespace TicketDesk.Web.Client
{
    /// <summary>
    /// This catalog applies a filtering expression in into a inner set
    /// and represents the subset. Note that it doesn't take ownership of the inner
    /// catalog, so if the inner needs to be disposed you have to do it yourself.
    /// </summary>
    public class FilteredCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        private readonly ComposablePartCatalog _inner;
        private readonly INotifyComposablePartCatalogChanged _innerNotifyChange;
        private readonly IQueryable<ComposablePartDefinition> _partsQuery;

        public FilteredCatalog(ComposablePartCatalog inner, Expression<Func<ComposablePartDefinition, bool>> expression)
        {
            _inner = inner;
            _innerNotifyChange = inner as INotifyComposablePartCatalogChanged;
            _partsQuery = inner.Parts.Where(expression);
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _partsQuery; }
        }

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed
        {
            add
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changed += value;
            }
            remove
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changed -= value;
            }
        }

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing
        {
            add
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changing += value;
            }
            remove
            {
                if (_innerNotifyChange != null)
                    _innerNotifyChange.Changing -= value;
            }
        }
    }
}