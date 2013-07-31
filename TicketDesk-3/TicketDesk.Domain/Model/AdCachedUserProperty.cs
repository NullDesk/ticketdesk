using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TicketDesk.Domain.Model
{
   
    
    public class AdCachedUserProperty
    {
        [Key]
        [Column(Order = 0)]
        [DisplayName("User Name")]
        [Required]
        [StringLength(150)]
        public string UserName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DisplayName("Property Name")]
        [Required]
        [StringLength(100)]
        public string PropertyName { get; set; }

        [DisplayName("Property Value")]
        [StringLength(250)]
        public string PropertyValue { get; set; }

        [DisplayName("Last Refreshed")]
        public DateTimeOffset? LastRefreshed { get; set; }

        [DisplayName("Is Active In Ad")]
        [Required]
        [DefaultValue(true)]
        public bool IsActiveInAd { get; set; }

    }
}
