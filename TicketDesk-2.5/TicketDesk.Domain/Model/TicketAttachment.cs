using System.ComponentModel;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketAttachment
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
        [StringLength(100)]
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
