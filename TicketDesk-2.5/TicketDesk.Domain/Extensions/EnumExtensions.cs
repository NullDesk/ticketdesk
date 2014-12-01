using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TicketDesk.Domain.Infrastructure;
using TicketDesk.Domain.Model;

namespace TicketDesk.Domain
{
    public static class EnumExtensions
    {

        /// <summary>
        /// Gets the description for an enum.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>The text of the DescriptionAttribute if applicable, otherwise returns the value as a string.</returns>
        /// <remarks>If used on a sepcifc value from the enumeration, will return the description for that value.
        /// If used on an enum as a whole, will return the description of the enum itself.</remarks>
        public static string GetDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var memberInfo = type.GetMember(enumeration.ToString());

            //if there is member information
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes.First()).Description;
                }
            }
            return enumeration.ToString();
        }


        /// <summary>
        /// Gets the enumerator from the description passed in
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description">The description.</param>
        /// <returns>T.</returns>
        public static T GetEnumFromDescription<T>(this string description)
        {
            //get the member info of the enum
            var memberInfos = typeof(T).GetMembers();

            if (memberInfos.Length > 0)
            {
                //loop through the member info classes
                foreach (var memberInfo in memberInfos)
                {
                    //get the custom attributes of the member info
                    object[] attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    //if there are attributes
                    if (attributes.Length > 0)
                        //if the description attribute is equal to the description, return the enum
                        if (((DescriptionAttribute)attributes[0]).Description == description)
                            return (T)Enum.Parse(typeof(T), memberInfo.Name);
                }
            }

            //this means the enum was not found from the description, so return the default
            return default(T);
        }

        public static IEnumerable<Enum> GetFlags(this Enum input)
        {
            return Enum.GetValues(input.GetType()).Cast<Enum>().Where(input.HasFlag);
        }
    }
}
