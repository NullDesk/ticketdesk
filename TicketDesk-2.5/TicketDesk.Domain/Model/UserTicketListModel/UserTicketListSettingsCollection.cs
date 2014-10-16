using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    /// <summary>
    /// Class JsonUserTicketListSettings.
    /// </summary>
    /// <remarks>
    /// This class is a mapped as a complex type, which allows storing the list settings to the DB as JSON, while treating the data in EF as regular entities
    /// </remarks>
    public class UserTicketListSettingsCollection: Collection<UserTicketListSetting>
    {
        public void Add(ICollection<UserTicketListSetting> settings)
        {
            foreach (var listSetting in settings)
            {
                this.Add(listSetting);
            }
        }

        public string Serialized
        {
            get { return Newtonsoft.Json.JsonConvert.SerializeObject(this); }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                var jData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserTicketListSetting>>(value);
                this.Items.Clear();
                this.Add(jData);
                
            }
        }
    }
}
