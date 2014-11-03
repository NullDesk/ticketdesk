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
using TicketDesk.Domain.Services;

namespace TicketDesk.Domain.Models
{
    public interface IProfile
    {
        /// <summary>
        /// Gets or sets the display preferences for the current user.
        /// </summary>
        /// <value>The display preferences for the current user.</value>
        UserDisplayPreferences DisplayPreferences { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to open editor with preview window visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if open editor with preview; otherwise, <c>false</c>.
        /// </value>
        bool OpenEditorWithPreview { get; set; }

        /// <summary>
        /// Gets or sets the user's editor mode.
        /// </summary>
        /// <value>The editor mode.</value>
        EditorModes EditorMode { get; set; }
    }
}
