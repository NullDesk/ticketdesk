using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TicketDesk.Localization.Identity;
using TicketDesk.Localization;

namespace TicketDesk.Web.Identity.Model
{
    public class TicketDeskRole : IdentityRole
    {
        private string displayName;
        private string description;

        public TicketDeskRole() : base() { }

        public TicketDeskRole(string roleName, string displayName, string description)
            : base(roleName)
        {
            this.displayName = displayName;
            this.description = description;
        }

        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "DisplayName", ResourceType = typeof(Strings))]
        public string DisplayName
        {
            get
            {
                if (TicketDeskRoleManager.DefaultRolesDisplayName.ContainsKey(this.Name))
                    return TicketDeskRoleManager.DefaultRolesDisplayName[this.Name];
                else
                    return this.displayName;
            }
            set
            {
                this.displayName = value;
            }
        }


        [StringLength(500, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "RoleDescription", ResourceType = typeof(Strings))]
        public string Description
        {
            get
            {
                if (TicketDeskRoleManager.DefaultRolesDescription.ContainsKey(this.Name))
                    return TicketDeskRoleManager.DefaultRolesDescription[this.Name];
                else
                    return this.displayName;
            }
            set
            {
                this.description = value;
            }
        }
    }
}
