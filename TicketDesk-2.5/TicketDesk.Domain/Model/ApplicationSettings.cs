using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ApplicationName { get; set; }

        public ApplicationPermissionsSetting Permissions { get; set; }

        public ApplicationSelectListSetting SelectLists { get; set; }
    }

}
