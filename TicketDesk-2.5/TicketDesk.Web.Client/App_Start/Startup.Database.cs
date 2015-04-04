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
using TicketDesk.Domain;
using TicketDesk.PushNotifications.Common;
using TicketDesk.Web.Identity;

namespace TicketDesk.Web.Client
{
    public partial class Startup
    {
        //TODO: while it is convienient to have all this set here, it may be more consistent to split the sub-domain db init up --perhaps move each into their own domain (identity and push notifications)
        public void ConfigureDatabase()
        {   //kill initializer - features that may need one will reset this later
            Database.SetInitializer<TdContext>(null);
            Database.SetInitializer<TdIdentityContext>(null);
            Database.SetInitializer<TdPushNotificationContext>(null);
            //set the std migrator for identity
            if (DatabaseConfig.IsDatabaseReady)
            {
                Database.SetInitializer(new TdIdentityDbInitializer());
                Database.SetInitializer(new TdPushNotificationDbInitializer());
            }
            DatabaseConfig.RegisterDatabase();

        }
    }
}