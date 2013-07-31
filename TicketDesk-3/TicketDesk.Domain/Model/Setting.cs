


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TicketDesk.Domain.Model
{
    [SettingValueMatchesType]
    public class Setting
    {
        [Key]
        [DisplayName("Setting Name")]
        [Required]
        [StringLength(50)]
        public string SettingName { get; set; }

        [DisplayName("Setting Value")]
        public string SettingValue { get; set; }

        [DisplayName("Default Value")]
        public string DefaultValue { get; set; }

        [DisplayName("Setting Type")]
        [Required]
        [StringLength(50)]
        [DefaultValue("SimpleString")]
        public string SettingType { get; set; }

        [DisplayName("Setting Description")]
        public string SettingDescription { get; set; }


    }

    public class SimpleSetting
    {
        public SimpleSetting(string settingValue)
        {
            Value = settingValue;
        }
        public string Value { get; set; }

    }
}
