using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TicketDesk.Domain.Model
{
    public class ApplicationPermissionsSetting
    {

        public ApplicationPermissionsSetting()
        {
            //To ensure that missing database values for settings do not completely brick the
            //  entire instance, make sure all default settings are initialized in the ctor
            AllowInternalUsersToEditPriority = false;
            AllowInternalUsersToEditTags = true;
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

                var jData = JsonConvert.DeserializeObject<ApplicationPermissionsSetting>(value);
                AllowInternalUsersToEditPriority = jData.AllowInternalUsersToEditPriority;
                AllowInternalUsersToEditTags = jData.AllowInternalUsersToEditTags;
            }
        }
        [NotMapped]
        public bool AllowInternalUsersToEditPriority { get; set; }

        [NotMapped]
        public bool AllowInternalUsersToEditTags { get; set; }
    }

}
