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
using System.Web;
using System.Web.Profile;
using TicketDesk.Domain.Models;
using TicketDesk.Domain.Services;
using System.ComponentModel.Composition;

namespace TicketDesk.Web.Client.Models
{
    /// <summary>
    /// Contains profile fields for TicketDesk users.
    /// </summary>
    /// <remarks>
    /// Web application projects do not auto-generate this class, so 
    /// it has to be manually created in order to interact with the 
    /// Profile object in the same way as would be done with a web site
    /// project.
    /// </remarks>
    [Export(typeof(IProfile))]
    public class ProfileCommon : ProfileBase, IProfile
    {

        
        /// <summary>
        /// Gets or sets the user's preferences.
        /// </summary>
        /// <value>The ticket list view settings.</value>
        public UserDisplayPreferences DisplayPreferences
        {
            get
            {
                var lv = (UserDisplayPreferences)HttpContext.Current.Profile.GetPropertyValue("UserDisplayPreferences");

                return lv;
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("UserDisplayPreferences", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to open editor with preview window visible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if open editor with preview; otherwise, <c>false</c>.
        /// </value>
        public bool OpenEditorWithPreview
        {
            get
            {
                return (bool)HttpContext.Current.Profile.GetPropertyValue("OpenEditorWithPreview");
            }
            set
            {
                HttpContext.Current.Profile.SetPropertyValue("OpenEditorWithPreview", value);
            }
        }

        /// <summary>
        /// Gets or sets the user's editor mode.
        /// </summary>
        /// <value>The editor mode.</value>
        public EditorModes EditorMode
        {
            get
            {
                return (EditorModes)HttpContext.Current.Profile.GetPropertyValue("EditorMode");
            }
            set
            {
                int v = (int)value;
                HttpContext.Current.Profile.SetPropertyValue("EditorMode", v.ToString());
            }
        }

    }

}