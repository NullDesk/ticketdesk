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
using System.Configuration;


namespace TicketDesk.Search.Common
{
    public class TdSearchContext
    {

        private static TdSearchContext _current;

        private static Func<TdSearchContextConfiguration> GetConfigurationFunc { get; set; }

        public static void Configure(Func<TdSearchContextConfiguration> getConfigurationFunc)
        {
            GetConfigurationFunc = getConfigurationFunc;
        }

        public static TdSearchContext Current
        {
            get
            {
                if (GetConfigurationFunc == null)
                {
                    throw new ConfigurationErrorsException("Attempt to access search system before TicketDeskSearchContext has been configured");
                }
                return _current ?? (_current = new TdSearchContext(GetConfigurationFunc()));
            }
        }
        
        internal TdSearchContext(TdSearchContextConfiguration configuration)
        {
            IndexSearcher = configuration.SearchLocatorProvider;
            IndexManager = configuration.SearchIndexProvider;
        }

        public ISearchLocatorProvider IndexSearcher { get; set; }

        public ISearchIndexProvider IndexManager { get; set; }
        
    }
}
