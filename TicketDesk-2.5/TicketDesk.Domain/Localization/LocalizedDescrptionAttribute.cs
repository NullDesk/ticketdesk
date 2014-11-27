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
