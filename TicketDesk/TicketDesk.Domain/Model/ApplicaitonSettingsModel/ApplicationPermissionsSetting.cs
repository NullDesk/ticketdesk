// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using TicketDesk.Localization.Domain;

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
            AllowInternalUsersToSetAssigned = false;
            AllowInternalUsersToSetOwner = false;
        }

        [JsonIgnore]
        [Display(AutoGenerateField = false)]
        [ScaffoldColumn(false)]
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
                var jData = JsonConvert.DeserializeObject<ApplicationPermissionsSetting>(value, jsettings);
                AllowInternalUsersToEditPriority = jData.AllowInternalUsersToEditPriority;
                AllowInternalUsersToEditTags = jData.AllowInternalUsersToEditTags;
                AllowInternalUsersToSetAssigned = jData.AllowInternalUsersToSetAssigned;
                AllowInternalUsersToSetOwner = jData.AllowInternalUsersToSetOwner;
            }
        }
        [NotMapped]
        [Display(Name = "AllowInternalUsersToEditPriority", ResourceType = typeof(Strings))]
        public bool AllowInternalUsersToEditPriority { get; set; }

        [NotMapped]
        [Display(Name = "AllowInternalUsersToEditTags", ResourceType = typeof(Strings))]
        public bool AllowInternalUsersToEditTags { get; set; }

        [NotMapped]
        [Display(Name = "AllowInternalUsersToSetAssigned", ResourceType = typeof(Strings))]
        public bool AllowInternalUsersToSetAssigned { get; set; }

        [NotMapped]
        [Display(Name = "AllowInternalUsersToSetOwner", ResourceType = typeof(Strings))]
        public bool AllowInternalUsersToSetOwner { get; set; }
    }

}
