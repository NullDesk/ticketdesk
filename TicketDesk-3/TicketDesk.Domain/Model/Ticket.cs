using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketDesk.Domain.Model.Localization;

namespace TicketDesk.Domain.Model
{
    public class Ticket
    {
        [Display(ResourceType = typeof(AppModelText), Name = "TicketTicketId")]
        [Required]
        public int TicketId { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketTicketType")]
        [Required]
        [StringLength(50)]
        public string TicketType { get; set; }


        [Display(ResourceType = typeof(AppModelText), Name = "TicketCategory")]

        [Required]
        [StringLength(50)]
        public string Category { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketTitle")]
        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketDetails")]
        [Required]
        [Column(TypeName = "ntext")]
        public string Details { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketIsHtml")]
        [Required]
        [DefaultValue(false)]
        public bool IsHtml { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketTagList")]
        [StringLength(100)]
        public string TagList { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketCreatedBy")]
        [Required]
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketCreatedDate")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTimeOffset CreatedDate { get; set; }

        private string _owner;

        [Display(ResourceType = typeof(AppModelText), Name = "TicketOwner")]
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


        [Display(ResourceType = typeof(AppModelText), Name = "TicketAssignedTo")]
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

        [Display(ResourceType = typeof(AppModelText), Name = "TicketCurrentStatus")]
        [Required]
        [StringLength(50)]
        public string CurrentStatus { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketCurrentStatusDate")]
        [Required]
        public DateTimeOffset CurrentStatusDate { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketCurrentStatusBy")]
        [Required]
        [StringLength(100)]
        public string CurrentStatusSetBy { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketLastUpdateBy")]
        [Required]
        [StringLength(100)]
        public string LastUpdateBy { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketLastUpdateDate")]
        [Required]
        public DateTimeOffset LastUpdateDate { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketPriority")]
        [StringLength(25)]
        public string Priority { get; set; }

        [Display(ResourceType = typeof(AppModelText), Name = "TicketAffectsCustomer")]
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

        [Display(ResourceType = typeof(AppModelText), Name = "TicketDetails")]
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
