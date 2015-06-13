using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketDesk.Web.Identity.Model
{
    public class TicketDeskRole : IdentityRole
    {
        public TicketDeskRole() : base() { }

        public TicketDeskRole(string roleName, string displayName, string description) : base(roleName)
        {
            DisplayName = displayName;
            Description = description;
        }

        [Required]
        [StringLength(100)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }


        [StringLength(500)]
        [Display(Name = "Role Description")]
        public string Description { get; set; }
    }
}
