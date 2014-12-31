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

using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace TicketDesk.Domain.Legacy.Migrations
{
    /// <summary>
    /// Class Configuration. This class cannot be inherited.
    /// </summary>
    public sealed class Configuration<T> : DbMigrationsConfiguration<T> where T : DbContext
    {
       
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "TicketDeskCore";
        }

        
    }
}
