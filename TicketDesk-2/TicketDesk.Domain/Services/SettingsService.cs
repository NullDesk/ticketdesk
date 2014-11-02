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
using System.Text;
using System.ComponentModel.Composition;

namespace TicketDesk.Domain.Services
{
    [Export]
    public class SettingsService
    {
        
        /// <summary>
        /// Unit test ctor, Initializes a new instance of the <see cref="TdSettingService"/> class.
        /// </summary>
        /// <param name="applicationSettings">The application settings.</param>
        /// <param name="userSettings">The user settings.</param>
        [ImportingConstructor]
        public SettingsService(IApplicationSettingsService applicationSettings, IUserSettingsService userSettings)
        {
            ApplicationSettings = applicationSettings;
            UserSettings = userSettings;
        }

        /// <summary>
        /// Gets or (private) sets the application settings.
        /// </summary>
        /// <value>The application settings.</value>
        public IApplicationSettingsService ApplicationSettings { get; private set; }

        /// <summary>
        /// Gets or (private) sets the user settings.
        /// </summary>
        /// <value>The user settings.</value>
        public IUserSettingsService UserSettings { get; private set; }
    }
    
}
