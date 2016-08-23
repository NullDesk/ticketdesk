using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketDesk.Localization;
using TicketDesk.Localization.Domain;

namespace TicketDesk.Domain.Model
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [StringLength(100, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Required(ErrorMessageResourceName = "FieldRequired", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "Project_Name", ResourceType = typeof(Strings))]
        public string ProjectName { get; set; }

        [StringLength(500, ErrorMessageResourceName = "FieldMaximumLength", ErrorMessageResourceType = typeof(Validation))]
        [Display(Name = "Project_Description", ResourceType = typeof(Strings))]
        public string ProjectDescription { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } 
    }
}
