// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using S22.Mail;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications
{
    [Description("SMTP Provider")]
    public sealed class SmtpDeliveryProvider : PushNotificationDeliveryProviderBase
    {
        public SmtpDeliveryProvider(JToken configuration)
        {
            Configuration = configuration == null ?
                new SmtpDeliveryProviderConfiguration() :
                configuration.ToObject<SmtpDeliveryProviderConfiguration>();
        }

        public override IDeliveryProviderConfiguration Configuration { get; set; }

        public override string DestinationType
        {
            get { return "email"; }
        }

        public override Task<object> GenerateMessageAsync(PushNotificationItem notificationItem, CancellationToken ct)
        {
            var memorydata = Convert.FromBase64String(notificationItem.MessageContent);
            using (var rs = new MemoryStream(memorydata))
            {
                var sf = new BinaryFormatter();
                return Task.FromResult(sf.Deserialize(rs));
            }
        }

        public override Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message, CancellationToken ct)
        {
            var cfg = (SmtpDeliveryProviderConfiguration)Configuration;
            var sent = false;
            //implicit conversion operator
            var smsg = message as SerializableMailMessage;
            if (smsg != null)
            {
                var client = new SmtpClient()
                {
                    Host = cfg.SmtpServer,
                    Port = cfg.SmtpPort ?? 25,
                    EnableSsl = cfg.EnableSsl??false
                };
                if (!string.IsNullOrEmpty(cfg.SmtpUserName))
                {
                    client.Credentials = new NetworkCredential(cfg.SmtpUserName, cfg.SmtpPassword);

                }
                smsg.To.Add(new MailAddress(notificationItem.Destination.DestinationAddress, notificationItem.Destination.SubscriberName));
                smsg.From = new MailAddress(cfg.SmtpFromAddress,cfg.SmtpFromDisplayName);
                client.Send(smsg);
                sent = true;
            }
            return Task.FromResult(sent);
        }
    }


}
