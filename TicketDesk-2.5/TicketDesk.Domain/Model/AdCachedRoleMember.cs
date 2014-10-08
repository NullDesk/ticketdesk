namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdCachedRoleMember
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string GroupName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        public string MemberName { get; set; }

        [Required]
        [StringLength(150)]
        public string MemberDisplayName { get; set; }
    }
}
