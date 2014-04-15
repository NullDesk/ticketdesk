using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

using Microsoft.AspNet.Identity.EntityFramework;

namespace TicketDesk.Domain.Model
{
    /// <summary>
    ///  User Profile entity
    /// </summary>
    [DataContract(IsReference = true)]
    public class UserProfile : IdentityUser
    {
        /// <summary>
        ///  Email for the User
        /// </summary>
        /// <remarks>Ignoring this field by not using DataMember</remarks>
        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(200)]
        [EmailAddress]
        [DataMember]
        public string Email { get; set; }
    }
}
