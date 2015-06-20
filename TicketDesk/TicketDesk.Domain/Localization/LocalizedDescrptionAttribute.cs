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
using System.Reflection;

namespace TicketDesk.Domain.Localization
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter)]
    class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        static string Localize(Type resourceType, string key)
        {
            var prop = resourceType.GetProperty(key, BindingFlags.Public | BindingFlags.Static);
            return (string)prop.GetValue(null);
        }
        
        public LocalizedDescriptionAttribute(Type resourceType,string key)
            : base(Localize(resourceType, key))
        {
        }
    }
}
