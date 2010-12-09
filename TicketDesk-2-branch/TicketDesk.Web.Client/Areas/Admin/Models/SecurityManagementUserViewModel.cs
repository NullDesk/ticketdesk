using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Web.Client.Areas.Admin.Models
{
    public class SecurityManagementUserViewModel : RegisterModel
    {


        [DisplayName("Approved")]
        public bool IsApproved { get; set; }

        [DisplayName("Is Admin")]
        public bool IsAdmin { get; set; }
        
        [DisplayName("Is Staff")]
        public bool IsStaff { get; set; }

        [DisplayName("Is Submitter")]
        public bool IsSubmitter { get; set; }
    }
}