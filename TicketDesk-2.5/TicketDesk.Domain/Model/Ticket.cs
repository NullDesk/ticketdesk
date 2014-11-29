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

using TicketDesk.Domain.Localization;

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

        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketTicketId", ShortName = "TicketTicketIdShort")]
        public int TicketId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketTicketType", ShortName = "TicketTicketTypeShort")]
        public string TicketType { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketCategory", ShortName = "TicketCategoryShort")]
        public string Category { get; set; }

        [Required]
        [StringLength(500)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketTitle", ShortName = "TicketTitleShort")]
        public string Title { get; set; }

        [Column(TypeName = "ntext")]
        [Required]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketDetails", ShortName = "TicketDetailsShort")]
        public string Details { get; set; }

        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketIsHtml", ShortName = "TicketIsHtmlShort")]
        public bool IsHtml { get; set; }

        [StringLength(100)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketTagList", ShortName = "TicketTagListShort")]
        public string TagList { get; set; }

        [Required]
        [StringLength(100)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketCreatedBy", ShortName = "TicketCreatedByShort")]
        public string CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketCreatedDate", ShortName = "TicketCreatedDateShort")]
        public DateTimeOffset CreatedDate { get; set; }

        private string _owner;


        [Required]
        [StringLength(100)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketOwner", ShortName = "TicketOwnerShort")]
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
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketAssignedTo", ShortName = "TicketAssignedToShort")]
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
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketTicketStatus", ShortName = "TicketTicketStatusShort")]
        public TicketStatus TicketStatus { get; set; }

        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketCurrentStatusDate", ShortName = "TicketCurrentStatusDateShort")]
        public DateTimeOffset CurrentStatusDate { get; set; }

        [Required]
        [StringLength(100)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketCurrentStatusSetBy", ShortName = "TicketCurrentStatusSetByShort")]
        public string CurrentStatusSetBy { get; set; }

        [Required]
        [StringLength(100)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketLastUpdateBy", ShortName = "TicketLastUpdateByShort")]
        public string LastUpdateBy { get; set; }
        
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketLastUpdateDate", ShortName = "TicketLastUpdateDateShort")]
        public DateTimeOffset LastUpdateDate { get; set; }

        [StringLength(25)]
        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketPriority", ShortName = "TicketPriorityShort")]
        public string Priority { get; set; }

        [Display(ResourceType = typeof(TicketDeskDomainText), Name = "TicketAffectsCustomer", ShortName = "TicketAffectsCustomerShort")]
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

        //internal string[] GetNotificationSubscribers()
        //{
        //    var subs = new List<string>();
        //    if (!string.IsNullOrEmpty(PreviousOwner) && PreviousOwner != Owner)
        //    {
        //        subs.Add(PreviousOwner);
        //    }
        //    if (!string.IsNullOrEmpty(PreviousAssignedUser) && PreviousAssignedUser != AssignedTo)
        //    {
        //        subs.Add(PreviousAssignedUser);
        //    }
        //    if (!string.IsNullOrEmpty(Owner))
        //    {
        //        subs.Add(Owner);
        //    }
        //    if (!string.IsNullOrEmpty(AssignedTo))
        //    {
        //        subs.Add(AssignedTo);
        //    }
        //    return subs.ToArray();
        //}

        [NotMapped]
        public bool IsAssigned
        {
            get { return !string.IsNullOrEmpty(AssignedTo); }
        }

        [NotMapped]
        public bool IsOpen
        {
            get { return TicketStatus != TicketStatus.Resolved && TicketStatus != TicketStatus.Closed; }
        }

        
        
    }
}
