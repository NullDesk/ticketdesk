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

namespace TicketDesk.Domain.Model
{
    public class ApplicationSetting
    {
        public ApplicationSetting()
        {
            //To ensure that missing database values for settings do not completely brick the
            //  entire instance, make sure all default settings are initialized in the ctor
            ApplicationName = "TicketDesk";
            Permissions = new ApplicationPermissionsSetting();
            SelectLists = new ApplicationSelectListSetting();
            SecuritySettings = new ApplicationSecuritySetting();

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(AutoGenerateField = false)]
        public string ApplicationName { get; set; }

        public ApplicationPermissionsSetting Permissions { get; set; }

        public ApplicationSelectListSetting SelectLists { get; set; }

        public ApplicationSecuritySetting SecuritySettings { get; set; }

      
    }

}
