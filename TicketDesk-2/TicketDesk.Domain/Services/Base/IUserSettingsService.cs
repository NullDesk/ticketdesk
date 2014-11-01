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

using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Services
{
    public interface IUserSettingsService
    {
        /// <summary>
        /// Gets or sets a value indicating whether to open editor with preview visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if open editor with preview; otherwise, <c>false</c>.
        /// </value>
        bool OpenEditorWithPreview { get; set; }

        /// <summary>
        /// Gets or sets the user's preferred editor mode.
        /// </summary>
        /// <value>The editor mode.</value>
        EditorModes EditorMode { get; set; }


        /// <summary>
        /// Gets the profile repository.
        /// </summary>
        /// <value>The repository.</value>
        IProfile Repository { get; }


        /// <summary>
        /// Gets the display preferences for the current user
        /// </summary>
        /// <returns></returns>
        UserDisplayPreferences GetDisplayPreferences();

        /// <summary>
        /// Saves the display preferences for the current user.
        /// </summary>
        /// <param name="prefrences">The preferences.</param>
        void SaveDisplayPreferences(UserDisplayPreferences prefrences);
    }
}
