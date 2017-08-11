// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (https://github.com/stephenredd)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://opensource.org/licenses/MS-PL
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.

using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using S22.Mail;
using SendGrid;
using TicketDesk.PushNotifications.Model;
using TicketDesk.Localization;
using TicketDesk.Localization.PushNotifications;
using System;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace TicketDesk.PushNotifications.Delivery
{
    [LocalizedDescription("SendGridProvider", NameResourceType = typeof(Strings))]
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
                        From = new EmailAddress(cfg.FromAddress, cfg.FromDisplayName),
                        Subject = smsg.Subject,
                        Contents = new List<Content>() {
                            new Content("text/html", hView.ContentStream.ReadToString()),
                            //new Content("text", tView.ContentStream.ReadToString())
                        },
                        Personalizations = new List<Personalization>()
                        {
                            new Personalization
                            {
                                Tos = new List<EmailAddress>()
                                {
                                    new EmailAddress(notificationItem.Destination.DestinationAddress, notificationItem.Destination.SubscriberName)
                                }                                
                            }
                        },
                    };

                    sendGridMessage.TrackingSettings = new TrackingSettings()
                    {
                        ClickTracking = new ClickTracking()
                    };

                    if (cfg.EnableClickTracking ?? false)
                    {
                        sendGridMessage.TrackingSettings.ClickTracking.Enable = true;;
                    }
                    if (cfg.EnableGravatar ?? false)
                    {
                        //sendGridMessage.MailSettings.EnableGravatar();
                    }
                    if (cfg.EnableOpenTracking ?? false)
                    {
                        sendGridMessage.TrackingSettings.OpenTracking.Enable = true;;
                    }
                    if (cfg.SendToSink ?? false)
                    {                        
                        //sendGridMessage.SendToSink();
                    }

                    //var transport = new Web(cfg.ApiKey);
                    //await transport.DeliverAsync(sendGridMessage);
                    //sent = true;

                    var sg = new SendGridClient(cfg.ApiKey);
                    var response = await sg.SendEmailAsync(sendGridMessage);
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
