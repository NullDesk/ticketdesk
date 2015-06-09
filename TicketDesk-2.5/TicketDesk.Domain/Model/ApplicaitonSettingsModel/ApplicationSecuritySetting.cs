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
    public class ApplicationSecuritySetting
    {
        public ApplicationSecuritySetting()
        {
            DefaultNewUserRoles = new List<string>(new[] {"TdPendingUsers"});
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
                var jsettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                var jData = JsonConvert.DeserializeObject<ApplicationSecuritySetting>(value, jsettings);
                DefaultNewUserRoles = jData.DefaultNewUserRoles;
            }
        }

        private ICollection<string> defaultNewUserRoles;



        [NotMapped]
        [Display(Name = "Default New User Roles")]
        public ICollection<string> DefaultNewUserRoles
        {
            get { return defaultNewUserRoles;}
            set
            {
                if (!value.Contains("TdPendingUsers"))
                {
                    value.Add("TdPendingUsers");
                }
                defaultNewUserRoles = value;
            }
        }
    }
}
