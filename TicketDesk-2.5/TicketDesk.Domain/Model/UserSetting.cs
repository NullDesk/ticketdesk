using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketDesk.Domain.Models;

namespace TicketDesk.Domain.Model
{
    using Json = Newtonsoft.Json;
    public class UserSetting
    {
        [Key]
        public string UserId { get; set; }

        [MaxLength]
        [Required]
        [Column]
        protected string ListSettingsJson { get; set; }


        private ICollection<UserTicketListSetting> listSettings;
        [NotMapped]
        public ICollection<UserTicketListSetting> ListSettings
        {
            get {
                return listSettings ??
                       (listSettings = Json.JsonConvert.DeserializeObject<List<UserTicketListSetting>>(ListSettingsJson));
            }
            set
            {
                listSettings = null;//clears the existing deser, will be rebuilt on get
                ListSettingsJson = Json.JsonConvert.SerializeObject(value);
            }
        }



        public static UserSetting GetDefaultSettingsForUser(string userId)
        {
            return new UserSetting {UserId = userId, ListSettings = UserTicketListSetting.GetDefaultListSettings()};
        }
    }
}
