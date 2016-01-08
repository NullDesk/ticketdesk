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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;


namespace TicketDesk.Domain.Model
{
    /// <summary>
    /// Class UserTicketListSettingsCollection.
    /// </summary>
    /// <remarks>
    /// This class is a mapped as a complex type, which allows storing the list settings 
    /// to the DB as JSON, while treating the data in EF as regular entities
    /// </remarks>
    public class UserTicketListSettingsCollection: Collection<UserTicketListSetting>
    {
        /// <summary>
        /// Adds the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Add(ICollection<UserTicketListSetting> settings)
        {
            foreach (var listSetting in settings)
            {
                Add(listSetting);
            }
        }

        /// <summary>
        /// Gets or sets the json serialized representation of the entire collection.
        /// </summary>
        /// <remarks>
        /// This is the only value in the entire sub-model for list 
        /// settings that will be stored by EF to the physical database.
        /// </remarks>
        /// <value>The serialized json settings collection.</value>
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
                
                var jsettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                var jData = JsonConvert.DeserializeObject<List<UserTicketListSetting>>(value, jsettings);
                Items.Clear();
                Add(jData);
                
            }
        }
    }
}
