using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketDesk.Domain.Model
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        public string UserName { get; set; }

        public string DisplayName { get; set; }
        
        //public object UserDisplayPreferences{get;set;} //TicketDesk.Domain.Models.UserDisplayPreferences" />

        [DefaultValue(true)]
        public bool? OpenEditorWithPreview { get; set; }
        
        [DefaultValue(2)]
        public int? EditorMode { get; set; }

    }
}
