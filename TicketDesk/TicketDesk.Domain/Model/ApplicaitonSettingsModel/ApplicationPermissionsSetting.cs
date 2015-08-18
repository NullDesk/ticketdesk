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
        [Display(Name = "Allow internal users to set the assigned to field (new ticket creation only)?")]
        public bool AllowInternalUsersToSetAssigned { get; set; }

        [NotMapped]
        [Display(Name = "Allow internal users to set or change the ticket owner?")]
        public bool AllowInternalUsersToSetOwner { get; set; }
    }

}
