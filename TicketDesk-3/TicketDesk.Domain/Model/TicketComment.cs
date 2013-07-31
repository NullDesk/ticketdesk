using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
    

namespace TicketDesk.Domain.Model
{
    
    public class TicketComment
    {
        [Key]
        [Column(Order = 0)]
        [DisplayName("Ticket Id")]
        [Required]
        public int TicketId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [DisplayName("Comment Id")]
        [Required]
        public int CommentId { get; set; }

        [DisplayName("Comment Event")]
        [StringLength(500)]
        public string CommentEvent { get; set; }

        [DisplayName("Comment")]
        [Column(TypeName="ntext")]
        public string Comment { get; set; }

        [DisplayName("Is Html")]
        [Required]
        public bool IsHtml { get; set; }

        [DisplayName("Commented By")]
        [Required]
        [StringLength(100)]
        public string CommentedBy { get; set; }

        [DisplayName("Commented Date")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset CommentedDate { get; set; }

        [DisplayName("Version")]
        [Required]
        [Timestamp]
        public byte[] Version { get; set; }
    
        public virtual Ticket Ticket { get; set; }

        public virtual ICollection<TicketEventNotification> TicketEventNotifications { get; set; }

        [NotMapped]
        public string HtmlComment
        {
            get
            {
                var md = new Markdown();
                return (this.IsHtml) ? this.Comment : md.Transform(this.Comment);
            }
        }



      
    }
}
