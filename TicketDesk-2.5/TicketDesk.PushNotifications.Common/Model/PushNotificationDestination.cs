using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketDesk.PushNotifications.Common.Model
{
    public class PushNotificationDestination
    {
        [Key]
        [Column(Order = 0)]
        public string SubscriberName { get; set; }
        [Key]
        [Column(Order = 1)]
        public string DestinationAddress { get; set; }
        [Key]
        [Column(Order = 2)]
        public string DestinationType { get; set; }
    }
}
