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
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TicketDesk.Domain.Models.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingValueMatchesType : ValidationAttribute
    {
        public string SettingValue { get; set; }
        public string SettingType { get; set; }



        public override bool IsValid(object value)
        {

            var isValid = false;
            Type objectType = value.GetType();
            var svProp = objectType.GetProperties().SingleOrDefault(p => p.Name == "SettingValue");
            var stProp = objectType.GetProperties().SingleOrDefault(p => p.Name == "SettingType");

            if (svProp != null && stProp != null)
            {
                SettingValue = (string)svProp.GetValue(value, null);
                SettingType = (string)stProp.GetValue(value, null);

            }

            switch (SettingType)
            {
                case "StringList":
                    isValid = CheckStringListFormatting(SettingValue);
                    if (!isValid)
                    {
                        ErrorMessage = "Setting is a comma seperated list of values. Must not start or end with spaces, and no spaces are allowed before or after the commas.";
                    }
                    break;
                case "SimpleString":
                    isValid = true;
                    break;
                case "BoolString":
                    isValid = (SettingValue == "false" || SettingValue == "true");
                    if (!isValid)
                    {
                        ErrorMessage = "Setting must be a \"true\" or \"false\", this is case sensitive!";
                    }
                    break;
                case "IntString":
                    try
                    {
                        var i = Convert.ToInt32(SettingValue);
                        isValid = true;
                    }
                    catch { }
                    if (!isValid)
                    {
                        ErrorMessage = "Setting must be a whole number.";
                    }
                    break;
                case "DoubleString":
                    try
                    {
                        var i = Convert.ToDouble(SettingValue);
                        isValid = true;
                    }
                    catch { }
                    if (!isValid)
                    {
                        ErrorMessage = "Setting must be a number (convertable to .NET dataype of System.Double).";
                    }
                    break;
                default:
                    break;
            }
            return isValid;
        }
        private bool CheckStringListFormatting(string value)
        {
            //just check if any of the elements in the list have leading or trailing spaces
            var v = false;

            var li = value.Split(',');
            foreach (var i in li)
            {
                v = !(i.StartsWith(" ") || i.EndsWith(" "));
                if (!v)
                {
                    break;
                }
            }

            return v;
        }
    }


}




