using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TicketDesk.Domain.Model
{


    public class AdCachedRoleMember
    {
        [Key]
        [Column(Order = 0)]
        [DisplayName("Group Name")]
        [Required]
        [StringLength(150)]
        public string GroupName { get; set; }

        [Key]
        [Column(Order = 1)]
        [DisplayName("Member Name")]
        [Required]
        [StringLength(150)]
        public string MemberName { get; set; }

        [DisplayName("Member Display Name")]
        [Required]
        [StringLength(150)]
        public string MemberDisplayName { get; set; }




    }
}
