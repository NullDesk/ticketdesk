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
