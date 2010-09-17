using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TicketDesk.Domain.Models.DataAnnotations;
using TicketDesk.Domain.Utilities;

namespace TicketDesk.Domain.Models
{
    [MetadataType(typeof(TicketMeta))]
    public partial class Ticket
    {
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


        internal string PreviousOwner { get; set; }
        internal string PreviousAssignedUser { get; set; }

        protected override void OnPropertyChanging(string property)
        {
            if (property == "AssignedTo")
            {
                PreviousAssignedUser = this.AssignedTo;
            }
            if (property == "Owner")
            {
                PreviousOwner = this.Owner;
            }
            base.OnPropertyChanging(property);
        }

        [DisplayName("Details")]
        public string HtmlDetails
        {
            get
            {
                var md = new Markdown();
                return (this.IsHtml) ? this.Details : md.Transform(this.Details, false);
            }
        }
    }
}
