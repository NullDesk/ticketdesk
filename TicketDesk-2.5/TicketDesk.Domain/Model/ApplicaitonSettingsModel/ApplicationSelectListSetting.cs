using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TicketDesk.Domain.Model
{
    public class ApplicationSelectListSetting
    {

        public ApplicationSelectListSetting()
        {
            //To ensure that missing database values for settings do not completely brick the
            //  entire instance, make sure all default settings are initialized in the ctor
            CategoryList = new List<string>(new[] { "Software", "Hardware", "Network" });
            PriorityList = new List<string>(new[] { "High", "Medium", "Low" });
            TicketTypesList = new List<string>(new[] { "Problem", "Question", "Request" });
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
                var jsettings = new JsonSerializerSettings() {ObjectCreationHandling = ObjectCreationHandling.Replace};
                var jData = JsonConvert.DeserializeObject<ApplicationSelectListSetting>(value, jsettings);
                CategoryList = jData.CategoryList;
                PriorityList = jData.PriorityList;
                TicketTypesList = jData.TicketTypesList;
            }
        }


        [NotMapped]
        public ICollection<string> CategoryList { get; set; }

        [NotMapped]
        public ICollection<string> PriorityList { get; set; }

        [NotMapped]
        public ICollection<string> TicketTypesList { get; set; }
    }

}
