using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketDesk.Domain.Annotations;

namespace TicketDesk.Domain.Model
{
    public class ClientSetting
    {
        public ClientSetting()
        {
            Settings = new Dictionary<string, string>();
        }

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
        public string Serialized
        {
            get { return JsonConvert.SerializeObject(this); }
            [UsedImplicitly]
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }
                var jsettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
                var jData = JsonConvert.DeserializeObject<ClientSetting>(value, jsettings);
                Settings = jData.Settings;
            }
        }

        [NotMapped]
        public IDictionary<string,string> Settings { get; set; }

    }
}
