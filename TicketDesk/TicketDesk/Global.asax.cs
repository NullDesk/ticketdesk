using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using TicketDesk.Engine;

namespace TicketDesk
{
    public class Global : System.Web.HttpApplication
    {
        private System.Timers.Timer emailDeliveryTimer;

        protected void Application_Start(object sender, EventArgs e)
        {
            string enabled = ConfigurationManager.AppSettings["EnableEmailNotifications"];
            if (!string.IsNullOrEmpty(enabled) && Convert.ToBoolean(enabled))
            {
                emailDeliveryTimer = new System.Timers.Timer();
                string deliveryMinutes = ConfigurationManager.AppSettings["EmailDeliveryTimerIntervalMinutes"];
                int interval = 300000;//5 minutes
                if (!string.IsNullOrEmpty(deliveryMinutes))
                {
                    interval = Convert.ToInt32(deliveryMinutes) * 60000;
                }
                emailDeliveryTimer.Elapsed += new System.Timers.ElapsedEventHandler(emailDeliveryTimer_Elapsed);
                emailDeliveryTimer.Interval = interval;
                emailDeliveryTimer.AutoReset = true;
                emailDeliveryTimer.Start();


            }
        }

        private void emailDeliveryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Object isRunningLock = new Object();
            lock (isRunningLock)
            {
                try
                {
                    NotificationService.SendWaitingTicketEventNotifications();


                }
                catch (Exception ex)
                {
                    Exception newEx = new ApplicationException("Failure in Email Delivery Timer", ex);
                    HttpContext mockContext = (new TicketDesk.Engine.MockHttpContext(false)).Context;
                    Elmah.ErrorLog.GetDefault(mockContext).Log(new Elmah.Error(newEx));
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}