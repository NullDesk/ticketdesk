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
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain.Model
{
    public enum TicketStatus
    {
        [Display(Name = "TicketStatusActive", ResourceType = typeof(Strings))]
        Active,
        [Display(Name = "TicketStatusMoreInfo", ResourceType = typeof(Strings))]
        MoreInfo,
        [Display(Name = "TicketStatusResolved", ResourceType = typeof(Strings))]
        Resolved,
        [Display(Name = "TicketStatusClosed", ResourceType = typeof(Strings))]
        Closed
    }
}
