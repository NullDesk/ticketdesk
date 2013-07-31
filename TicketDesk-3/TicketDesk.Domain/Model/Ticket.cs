using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketDesk.Domain.Model
{
    public class Ticket
    {
        [DisplayName("Ticket Id")]
        [Required]
        public int TicketId { get; set; }

        [DisplayName("Type")]
        [Required]
        [StringLength(50)]
        public string TicketType { get; set; }

        [DisplayName("Category")]
        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [DisplayName("Title")]
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [DisplayName("Details")]
        [Required]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        [DisplayName("Is Html")]
        [Required]
        [DefaultValue(false)]
        public bool IsHtml { get; set; }

        [DisplayName("Tag List")]
        [StringLength(100)]
        public string TagList { get; set; }

        [DisplayName("Created By")]
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [DisplayName("Created Date")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset CreatedDate { get; set; }

        private string _owner;

        [DisplayName("Owner")]
        [Required]
        [StringLength(100)]
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                PreviousOwner = this._owner;
                _owner = value;
            }
        }
        
        private string _assignedTo;

        [DisplayName("Assigned To")]
        [StringLength(100)]
        public string AssignedTo
        {
            get
            {
                return _assignedTo;
            }
            set
            {
                PreviousAssignedUser = this._assignedTo;
                _assignedTo = value;
            }
        }

        [DisplayName("Current Status")]
        [Required]
        [StringLength(50)]
        public string CurrentStatus { get; set; }

        [DisplayName("Current Status Date")]
        [Required]
        public DateTimeOffset CurrentStatusDate { get; set; }

        [DisplayName("Current Status Set By")]
        [Required]
        [StringLength(100)]
        public string CurrentStatusSetBy { get; set; }

        [DisplayName("Last Update By")]
        [Required]
        [StringLength(100)]
        public string LastUpdateBy { get; set; }

        [DisplayName("Last Update Date")]
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }

        [DisplayName("Priority")]
        [StringLength(25)]
        public string Priority { get; set; }

        [DisplayName("Affects Customer")]
        [Required]
        [DefaultValue(false)]
        public bool AffectsCustomer { get; set; }

        [DisplayName("Published To Kb")]
        [Required]
        [DefaultValue(false)]
        public bool PublishedToKb { get; set; }

        [DisplayName("Version")]
        [Timestamp]
        public byte[] Version { get; set; }

        [DisplayName("Attachments")]
        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }
        
        public virtual ICollection<TicketTag> TicketTags { get; set; }

        internal string[] GetNotificationSubscribers()
        {
            List<string> subs = new List<string>();
            if (!string.IsNullOrEmpty(PreviousOwner) && PreviousOwner != Owner)
            {
                subs.Add(PreviousOwner);
            }
            if (!string.IsNullOrEmpty(PreviousAssignedUser) && PreviousAssignedUser != AssignedTo)
            {
                subs.Add(PreviousAssignedUser);
            }
            if (!string.IsNullOrEmpty(Owner))
            {
                subs.Add(Owner);
            }
            if (!string.IsNullOrEmpty(AssignedTo))
            {
                subs.Add(AssignedTo);
            }
            return subs.ToArray();
        }

        [DisplayName("Details")]
        [NotMapped]
        public string HtmlDetails
        {
            get
            {
                var md = new Markdown();
                return (this.IsHtml) ? this.Details : md.Transform(this.Details);
            }
        }

        [NotMapped]
        internal string PreviousOwner { get; set; }

        [NotMapped]
        internal string PreviousAssignedUser { get; set; }
    }
}
