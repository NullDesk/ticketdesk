using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using S22.Mail;
using SendGrid;
using TicketDesk.PushNotifications.Model;

namespace TicketDesk.PushNotifications.Delivery
{
    [Description("SendGrid Provider (Email)")]
    public sealed class SendGridDeliveryProvider : EmailDeliveryProviderBase
    {
        public SendGridDeliveryProvider(JToken configuration)
        {
            Configuration = configuration == null ?
                new SendGridDeliveryProviderConfiguration() :
                configuration.ToObject<SendGridDeliveryProviderConfiguration>();
        }

        public override async Task<bool> SendNotificationAsync(PushNotificationItem notificationItem, object message, CancellationToken ct)
        {
            var cfg = (SendGridDeliveryProviderConfiguration)Configuration;
            var sent = false;
            //implicit conversion operator
            MailMessage smsg = message as SerializableMailMessage;
            if (smsg != null)
            {
                try
                {
                    var hView = smsg.AlternateViews.First(v => v.ContentType.MediaType == "text/html");
                    var tView = smsg.AlternateViews.First(v => v.ContentType.MediaType == "text/plain");
                    var sendGridMessage = new SendGridMessage
                    {
                        To = new[]
                        {
                            new MailAddress(notificationItem.Destination.DestinationAddress,
                                notificationItem.Destination.SubscriberName)
                        },
                        From = new MailAddress(cfg.FromAddress, cfg.FromDisplayName),
                        Subject = smsg.Subject,
                        Html = hView.ContentStream.ReadToString(),
                        Text = tView.ContentStream.ReadToString()
                    };

                    if (cfg.EnableClickTracking ?? false)
                    {
                        sendGridMessage.EnableClickTracking();
                    }
                    if (cfg.EnableGravatar ?? false)
                    {
                        sendGridMessage.EnableGravatar();
                    }
                    if (cfg.EnableOpenTracking ?? false)
                    {
                        sendGridMessage.EnableOpenTracking();
                    }
                    if (cfg.SendToSink ?? false)
                    {
                        sendGridMessage.SendToSink();
                    }

                    var transport = new Web(cfg.ApiKey);
                    await transport.DeliverAsync(sendGridMessage);
                    sent = true;
                }
                catch
                {
                    sent = false;
                    //TODO: log this somewhere
                }

            }
            return sent;
        }


    }

    public static class StreamExtensions
    {
        public static string ReadToString(this Stream contentStream)
        {
            using (var reader = new StreamReader(contentStream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
