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

using System.Web.Mvc;
using TicketDesk.Domain;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        public void ConfigureSearch()
        {

            var context = DependencyResolver.Current.GetService<TicketDeskContext>();

            context.SearchProvider.InitializeSearch().ConfigureAwait(false);//don't await this, it'll run in the background

        }
    }
}