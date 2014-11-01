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
using System.Collections.Specialized;

namespace TicketDesk.Domain.Models
{
    public class RuleException : Exception
    {
        public NameValueCollection Errors { get; private set; }

        public RuleException(string key, string value)
        {
            Errors = new NameValueCollection { { key, value } };
        }

        public RuleException(NameValueCollection errors)
        {
            Errors = errors;
        }

       
    }
}
