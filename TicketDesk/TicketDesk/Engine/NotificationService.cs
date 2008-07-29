using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using TicketDesk.Controls;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Engine
{
    public static class NotificationService
    {
        delegate int TicketNotificationDelegate(int ticketid, string url);

        static List<int> CurrentNotifications = new List<int>();

        public static void Notification(int ticketid, string url)
        {
            if (!CurrentNotifications.Contains(ticketid))
            {
                CurrentNotifications.Add(ticketid);
                TicketNotificationDelegate tnd = new TicketNotificationDelegate(TicketDeskServiceUtilities.BeginNotificationCycle);
                //AsyncCallback cb = new AsyncCallback(RemoveCurrentNotification);
                tnd.BeginInvoke(ticketid, url, RemoveCurrentNotification, tnd);
            }
        }

        static void RemoveCurrentNotification(IAsyncResult ar)
        {
            TicketNotificationDelegate tnd = (TicketNotificationDelegate)ar.AsyncState;

            int ticketid = tnd.EndInvoke(ar);

            CurrentNotifications.Remove(ticketid);
        } 
    }
}
