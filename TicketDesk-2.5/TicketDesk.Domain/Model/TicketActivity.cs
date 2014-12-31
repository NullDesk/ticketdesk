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

using System;
using System.ComponentModel;
using TicketDesk.Domain.Infrastructure;

namespace TicketDesk.Domain.Model
{

    [Flags]
    public enum TicketActivity
    {
        None = 0,

        [CommentRequired]
        [Description("Comment")]
        AddComment = 1,

        [CommentRequired]
        [Description("Provide Info")]
        SupplyMoreInfo = 2,

        [Description("Cancel More Info")]
        CancelMoreInfo = 4,

        [CommentRequired]
        [Description("Request More Info")]
        RequestMoreInfo = 8,

        [Description("Take Over")]
        TakeOver = 16,

        [CommentRequired]
        [Description("Resolve")]
        Resolve = 32,

        [Description("Assign")]
        Assign = 64,

        [Description("Re-assign")]
        ReAssign = 128,

        [Description("Pass")]
        Pass = 256,

        [Description("Close")]
        Close = 512,

        [CommentRequired]
        [Description("Re-open")]
        ReOpen = 1024,

        [Description("Give Up")]
        [CommentRequired]
        GiveUp = 2048,

        [CommentRequired]
        [Description("Force Close")]
        ForceClose = 4096,

        [Description("Edit Attachments")]
        ModifyAttachments = 8192,

        [Description("Edit")]
        EditTicketInfo = 16384,

        [Description("Create")]
        Create = 32768,

        [Description("Create")]
        CreateOnBehalfOf = 65536
    }


}
