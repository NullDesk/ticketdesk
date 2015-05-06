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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace TicketDesk.PushNotifications.Common.Model
{
    public class PushNotificationDestinationCollection : Collection<PushNotificationDestination>
    {
        public void Add(ICollection<PushNotificationDestination> settings)
        {
            foreach (var listSetting in settings)
            {
                Add(listSetting);
            }
        }


        [JsonIgnore]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var jsettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                var jData = JsonConvert.DeserializeObject<List<PushNotificationDestination>>(value, jsettings);
                Items.Clear();
                Add(jData);

            }
        }
    }
}
