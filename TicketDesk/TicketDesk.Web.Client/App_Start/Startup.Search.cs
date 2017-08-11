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
using System.Linq;
using System.Web.Hosting;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using TicketDesk.Search.Azure;
using TicketDesk.Search.Common;
using TicketDesk.Search.Lucene;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {

        public void ConfigureSearch()
        {
            TdSearchContext.Configure(GetSearchConfiguration);

            TdSearchContext.Current.IndexManager.RunIndexMaintenanceAsync();

            //register for static ticket changed event handler 
            TdDomainContext.TicketsChanged += (sender, ticketChanges) =>
            {
                // ReSharper disable once EmptyGeneralCatchClause
                try
                {
                    var changes = ticketChanges as Ticket[] ?? ticketChanges.ToArray();
                    //deletes (such as the demo data manager removing everything) will fire 
                    //  this, but ticketChanges will be empty in those cases
                    if (changes.Any())
                    {
                        //add to search index
                        var searchItems = changes.ToSeachIndexItems().ToArray();

                        HostingEnvironment.QueueBackgroundWorkItem(
                            async ct =>
                                await
                                    TdSearchContext.Current.IndexManager.AddItemsToIndexAsync(searchItems)
                                        .ConfigureAwait(false));


                    }
                }
                catch
                {
                    //TODO: log this somewhere                    
                }
            };
        }


        /// <summary>
        /// Gets a search configuration by scanning available index providers in order of preference 
        /// and taking the first available one found.
        /// </summary>
        /// <remarks>
        /// This is supplied to the search system as a func and invoked when needed.
        /// </remarks>
        /// <returns>SearchContextConfiguration.</returns>
        private TdSearchContextConfiguration GetSearchConfiguration()
        {
            //TODO: when we move to a plug-in model, this should be refactored to use an applicaiton setting.
            //  Application settings will determine the order of preference for providers
            //  Func will cheack each in order, returing the first match found.
            //  By default, the system will assume that any custom plug-ins installed are 
            //  preferred (in alphabetical order if more than one), then Azure, and finally 
            //  Lucene as a last resort provider.
            //
            //  It would be nice if there were a way to detect if a provider was configured 
            //  and ready for use without fully instantiating the provider, but this may be
            //  cumbersome since interfaces can't specify static members, and any other 
            //  mechanism seems like it would be somewhat convoluted for would-be plug-in 
            //  autors to implement.

            var azProvider = new AzureIndexProvider();
            var config = azProvider.IsConfigured ?
                new TdSearchContextConfiguration(azProvider, new AzureSearchLocatorProvider()) :
                new TdSearchContextConfiguration(new LuceneIndexProvider(), new LuceneSearchLocatorProvider());
            return config;
        }

    }
}