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
using TicketDesk.Domain.Models;
using System.Web.Mvc;

namespace TicketDesk.Web.Client.Helpers
{
    public static class RuleExceptionExtensions
    {

        public static void CopyToModelState(this RuleException ruleException, ModelStateDictionary modelState, string prefix)
        {
            foreach (string errorKey in ruleException.Errors)
            {
                foreach (string errorValue in ruleException.Errors.GetValues(errorKey))
                {
                    var key = errorKey;
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        key = prefix + "." + key;
                    }
                    modelState.AddModelError(key, errorValue);
                }
            }
        }
    }
}