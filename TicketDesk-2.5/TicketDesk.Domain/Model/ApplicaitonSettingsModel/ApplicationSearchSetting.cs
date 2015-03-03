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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TicketDesk.Domain.Search;

namespace TicketDesk.Domain.Model
{
    public class ApplicationSearchSetting
    {
        public ApplicationSearchSetting()
        {
            SearchMode = ApplicationSearchMode.Auto;
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
                SearchMode = jData.SearchMode;
                SearchIndexName = jData.SearchIndexName;
                
            }
        }

        [NotMapped]
        [Display(Name = "Search Mode")]
        public ApplicationSearchMode SearchMode { get; set; }

        [NotMapped]
        [Display(Name = "Search Index Name")]
        public string SearchIndexName { get; set; }

       
    

    }

    
}
