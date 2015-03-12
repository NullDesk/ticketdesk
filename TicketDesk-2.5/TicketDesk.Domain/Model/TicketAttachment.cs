// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TicketDesk.Domain.Model
{


    public class TicketAttachment
    {
        [DisplayName("Ticket Id")]
        public int? TicketId { get; set; }

        [Key]
        [DisplayName("File Id")]
        public int FileId { get; set; }

        [Required]
        [StringLength(255)]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [DisplayName("File Size")]
        public int FileSize { get; set; }

        [Required]
        [StringLength(250)]
        [DisplayName("File Type")]
        public string FileType { get; set; }

        [Required]
        [StringLength(256)]
        [DisplayName("Uploaded By")]
        public string UploadedBy { get; set; }

        [DisplayName("Uploaded Date")]
        public DateTimeOffset UploadedDate { get; set; }

        [Required]
        [DisplayName("File Contents")]
        public byte[] FileContents { get; set; }

        [StringLength(500)]
        [DisplayName("File Description")]
        public string FileDescription { get; set; }

        [DisplayName("Is Pending")]
        public bool IsPending { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
