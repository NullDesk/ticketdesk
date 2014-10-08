namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AdCachedUserProperty
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(150)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string PropertyName { get; set; }

        [StringLength(250)]
        public string PropertyValue { get; set; }

        public DateTime? LastRefreshed { get; set; }

        public bool IsActiveInAd { get; set; }
    }
}
