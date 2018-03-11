using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLAWatchdog
{
    public class WatchdogThreads
    {
        public static void startWatch()
        {
            int numPriority = 3; //number of priorities. Want to set this in one place
            int sleepTime = 300000; //microseconds
            for (int i = 0; i < numPriority; i++)
            {
                Thread watchThread = new Thread(() => threadActions("high", true, sleepTime));
            }
        }

        public static void threadActions(string priority, bool firstStartup, int sleepTime)
        {
            DateTime suspendCheckTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);
            DateTime resumeCheckTieme = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 19, 00, 00);
            DateTime currTime;
            TimeSpan diff;
            //these threads should be always checking the db status and sending alerts
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
                if (DateTime.Compare(currTime, suspendCheckTime) >= 0)
                {
                    //past 6 PM suspend checking until the next morning
                    //assuming no thread will wake past 7PM
                    diff = resumeCheckTieme - currTime;
                    Thread.Sleep(46800 * 1000 + (int)diff.TotalMilliseconds);
                }
                //check for sla violation
                Thread.Sleep(sleepTime);
            }
        }
    }
}