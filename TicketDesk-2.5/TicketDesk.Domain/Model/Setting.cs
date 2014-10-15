// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

namespace TicketDesk.Domain.Model
{
    using System.ComponentModel.DataAnnotations;

    public class Setting
    {
        [Key]
        [StringLength(50)]
        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        public string DefaultValue { get; set; }

        [Required]
        [StringLength(50)]
        public string SettingType { get; set; }

        public string SettingDescription { get; set; }
    }
}
