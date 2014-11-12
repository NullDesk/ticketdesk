// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://ticketdesk.codeplex.com/license
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System.ComponentModel;

namespace TicketDesk.Domain.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Ticket
    {
        public Ticket()
        {
            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            TicketAttachments = new HashSet<TicketAttachment>();
            TicketComments = new HashSet<TicketComment>();
            TicketTags = new HashSet<TicketTag>();
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        [DisplayName("Ticket Id")]
        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Type")]
        public string TicketType { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Category")]
        public string Category { get; set; }

        [Required]
        [StringLength(500)]
        [DisplayName("Title")]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        [DisplayName("Details")]
        public string Details { get; set; }

        [DisplayName("Is Html")]
        public bool IsHtml { get; set; }

        [StringLength(100)]
        [DisplayName("Tags")]
        public string TagList { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Created Date")]
        public DateTimeOffset CreatedDate { get; set; }

        private string _owner;


        [Required]
        [StringLength(100)]
        [DisplayName("Owner")]
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                PreviousOwner = _owner;
                _owner = value;
            }
        }

        private string _assignedTo;

        [StringLength(100)]
        [DisplayName("Assigned To")]
        public string AssignedTo
        {
            get
            {
                return _assignedTo;
            }
            set
            {
                PreviousAssignedUser = _assignedTo;
                _assignedTo = value;
            }
        }

        [Required]
        [DisplayName("Status")]
        public TicketStatus TicketStatus { get; set; }

        [DisplayName("Status Date")]
        public DateTimeOffset CurrentStatusDate { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Status By")]
        public string CurrentStatusSetBy { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Updated By")]
        public string LastUpdateBy { get; set; }
        
        [DisplayName("Updated Date")]
        public DateTimeOffset LastUpdateDate { get; set; }

        [StringLength(25)]
        [DisplayName("Priority")]
        public string Priority { get; set; }

        
        public bool AffectsCustomer { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }

        public virtual ICollection<TicketComment> TicketComments { get; set; }

        public virtual ICollection<TicketTag> TicketTags { get; set; }


        [NotMapped]
        internal string PreviousOwner { get; set; }

        [NotMapped]
        internal string PreviousAssignedUser { get; set; }

        internal string[] GetNotificationSubscribers()
        {
            var subs = new List<string>();
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
    }
}
