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
