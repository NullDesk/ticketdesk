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

namespace TicketDesk.Domain.Model
{
    public class ApplicationSearchSetting
    {
        public ApplicationSearchSetting()
        {
            SearchIndexName = "ticketdesk-searchindex";
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
                var jData = JsonConvert.DeserializeObject<ApplicationSearchSetting>(value, jsettings);
                SearchIndexName = jData.SearchIndexName;
                
            }
        }

        [NotMapped]
        [Display(Name = "Search Index Name")]
        public string SearchIndexName { get; set; }

       
    

    }

    
}
