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

namespace TicketDesk.Domain.Services
{
    /// <summary>
    /// Specifies a text editing mode
    /// </summary>
    public enum EditorModes
    {
        /// <summary>
        /// Rich HTML WYSIWYG Editor (TinyMCE or CKEditor)
        /// </summary>
        RichHtmlEditor = 0,
        /// <summary>
        /// Markdown Editor (MarkItUp or WMD in Markdown mode)
        /// </summary>
        MarkDownEditor = 1,
        /// <summary>
        /// Plain Text (TextArea)
        /// </summary>
        PlainTextEditor = 2
    }
}
