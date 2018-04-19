using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicketDesk.Domain.Model;

using TicketDesk.Domain;

namespace ngWebClientAPI
{
    public class WatchdogThreads
    {
        private static TdDomainContext Context { get; set; }
        public WatchdogThreads(TdDomainContext context)
        {
            Context = context;
        }
        public static void StartWatch()
        {
            int numPriority = GlobalConfig.SLASettings.Count; //number of priorities

            int baseSleep = 60000;
            foreach (var item in GlobalConfig.SLASettings)
            {
                Thread watchThread = new Thread(() => threadActions(item.Key, true, baseSleep * item.Value));
            }
        }

        public static void threadActions(string priority, bool firstStartup, int sleepTime)
        {
            DateTimeOffset suspendCheckTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);
            DateTimeOffset resumeCheckTieme = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 00);
            DateTimeOffset currTime;
            TimeSpan diff;
            // these threads should be always checking the db status and sending alerts
            // they will sleep for longer if not in normal business hours i.e. 6 PM to 8 AM
            while (true)
            {
                if (firstStartup)
                {
                    //threads will sleep longer if application has been first started
                    firstStartup = false; //once past first startup want to skip longer sleep
                    Thread.Sleep(1);//to be changed
                    continue;
                }
                currTime = DateTime.Now;
                if (DateTimeOffset.Compare(currTime, suspendCheckTime) >= 0)
                {
                    //past 6 PM suspend checking until the next morning
                    //assuming no thread will wake past 7PM
                    diff = resumeCheckTieme - currTime;
                    Thread.Sleep(46800 * 1000 + (int)diff.TotalMilliseconds);
                }

                //this query should always hit the DB because its calling ToList
                List<Ticket> openTickets = Context.Tickets.Where(t => t.TicketStatus == TicketStatus.Active && t.Priority == priority).ToList();
                if(openTickets == null)
                {
                    foreach (var ticket in openTickets)
                    {
                        if (ticket.LastUpdateDate.AddMinutes(45) < currTime)
                        {
                            //SLA violation has occurred
                            //send alert
                        }
                    }
                }
                else
                {
                    Thread.Sleep(sleepTime);
                }
            }
        }
    }
}