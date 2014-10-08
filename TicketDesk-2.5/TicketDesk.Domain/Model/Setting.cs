namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Setting
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
