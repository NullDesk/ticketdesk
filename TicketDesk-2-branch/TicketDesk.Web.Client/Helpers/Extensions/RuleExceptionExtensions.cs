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