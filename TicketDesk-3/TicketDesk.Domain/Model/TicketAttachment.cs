using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TicketDesk.Domain.Model
{
    public class TicketAttachment
    {
        [DisplayName("Ticket Id")]
        public int? TicketId { get; set; }

        [Key]
        [DisplayName("File Id")]
        [Required]
        public int FileId { get; set; }

        [DisplayName("File Name")]
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [DisplayName("File Size")]
        [Required]
        public int FileSize { get; set; }

        [DisplayName("File Type")]
        [Required]
        [StringLength(250)]
        public string FileType { get; set; }

        [DisplayName("Uploaded By")]
        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; }

        [DisplayName("Uploaded Date")]
        [Required]
        public DateTimeOffset UploadedDate { get; set; }

        [DisplayName("File Contents")]
        [Required]
        public byte[] FileContents { get; set; }

        [DisplayName("File Description")]
        [StringLength(500)]
        public string FileDescription { get; set; }

        [DisplayName("Is Pending")]
        [Required]
        [DefaultValue(false)]
        public bool IsPending { get; set; }
    
        public virtual Ticket Ticket { get; set; }


       
    }
}
