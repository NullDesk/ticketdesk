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


namespace TicketDesk.Search.Common
{
    public class SearchContextConfiguration
    {
        public SearchContextConfiguration(ISearchIndexProvider indexProvider, ISearchLocatorProvider locatorProvider)
        {
            SearchIndexProvider = indexProvider;
            SearchLocatorProvider = locatorProvider;
        }

        public ISearchIndexProvider SearchIndexProvider { get; set; }
        public ISearchLocatorProvider SearchLocatorProvider { get; set; }
    }
}
