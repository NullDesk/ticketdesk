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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Mvc;
using Postal;
using S22.Mail;
using TicketDesk.PushNotifications.Model;
using TicketDesk.Web.Client.Models;

namespace TicketDesk.Domain.Model
{
    public static class TicketEventNotificationExtensions
    {
        private static string _rootUrl;
        private static string RootUrl
        {
            get
            {
                if (_rootUrl == null)
                {
                    using (var context = DependencyResolver.Current.GetService<TdDomainContext>())
                    {
                        _rootUrl = context.TicketDeskSettings.ClientSettings.GetDefaultSiteRootUrl();
                    }
                }
                return _rootUrl;
            }

        }

        public static IEnumerable<NewTicketPushNotificationInfo> ToNewTicketPushNotificationInfoCollection(
            this IEnumerable<TicketEventNotification> eventNotifications,
            bool multiProject
        )
        {
            return eventNotifications.Select(note => new NewTicketPushNotificationInfo()
            {
                TicketId = note.TicketId,
                MessageContent = GetEmailForNote(note, multiProject, true)
            });
        }

        public static IEnumerable<TicketPushNotificationEventInfo> ToNotificationEventInfoCollection(
            this IEnumerable<TicketEventNotification> eventNotifications,
            bool subscriberExclude,
            bool multiProject)
        {

            return eventNotifications.Select(note => new TicketPushNotificationEventInfo()
            {
                TicketId = note.TicketId,
                SubscriberId = note.SubscriberId,
                EventId = note.EventId,
                CancelNotification = subscriberExclude && note.IsRead,
                MessageContent = GetEmailForNote(note, multiProject)
            });

        }

        private static string GetEmailForNote(TicketEventNotification note, bool multiProject, bool forNewTicket = false)
        {

            var email = new TicketEmail
            {
                Ticket = note.TicketEvent.Ticket,
                SiteRootUrl = RootUrl,
                IsMultiProject = multiProject,
                ForNewTicket = forNewTicket
            };
            var mailService = new EmailService();
            SerializableMailMessage message = mailService.CreateMailMessage(email);
            using (var ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, message);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }
}
