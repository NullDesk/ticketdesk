// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
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
using System.Configuration;
using System.Threading.Tasks;
using TicketDesk.Search.Common;


namespace TicketDesk.Search
{
    public class TicketDeskSearchContext
    {

        private static TicketDeskSearchContext _current;

        private static Func<SearchContextConfiguration> GetConfigurationFunc { get; set; }

        public static void Configure(Func<SearchContextConfiguration> getConfigurationFunc)
        {
            GetConfigurationFunc = getConfigurationFunc;
        }

        public static TicketDeskSearchContext Current
        {
            get
            {
                if (GetConfigurationFunc == null)
                {
                    throw new ConfigurationErrorsException("Attempt to access search system before TicketDeskSearchContext has been configured");
                }
                return _current ?? (_current = new TicketDeskSearchContext(GetConfigurationFunc()));
            }
        }
        
        internal TicketDeskSearchContext(SearchContextConfiguration configuration)
        {
            IndexSearcher = configuration.SearchLocatorProvider;
            IndexManager = configuration.SearchIndexProvider;
        }

        public ISearchLocatorProvider IndexSearcher { get; set; }

        public ISearchIndexProvider IndexManager { get; set; }
        
    }
}
