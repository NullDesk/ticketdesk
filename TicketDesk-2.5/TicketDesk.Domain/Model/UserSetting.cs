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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    
    public class UserSetting
    {
        [Key]
        public string UserId { get; set; }


        public virtual UserTicketListSettingsCollection ListSettings { get; set; }
       

        public UserTicketListSetting GetUserListSettingByName(string listName)
        {
            return ListSettings.FirstOrDefault(us => us.ListName == listName);
        }

        public static UserSetting GetDefaultSettingsForUser(string userId)
        {
            var collection = new UserTicketListSettingsCollection
            {
                UserTicketListSetting.GetDefaultListSettings()
            };

            return new UserSetting { UserId = userId, ListSettings = collection };
        }
    }


}
