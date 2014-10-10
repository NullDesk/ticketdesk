namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TicketAttachment
    {
        public int? TicketId { get; set; }

        [Key]
        public int FileId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public int FileSize { get; set; }

        [Required]
        [StringLength(250)]
        public string FileType { get; set; }

        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; }

        public DateTimeOffset UploadedDate { get; set; }

        [Required]
        public byte[] FileContents { get; set; }

        [StringLength(500)]
        public string FileDescription { get; set; }

        public bool IsPending { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
