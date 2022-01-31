using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Timers;
using System.Web.Hosting;
using Ticketing_Dashboard.Models;

namespace Ticketing_Dashboard.Utility
{
    public class Scheduler
    {
        private static Timer timer;

        public static void Main()
        {
            Scheduler.schedule_Timer();
            File.WriteAllText(HostingEnvironment.MapPath("~/App_Data/SchedulerCounter.txt"), "running");
        }

        public static bool CheckSchedulerStatus() => File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/SchedulerCounter.txt")).Trim() == "1";

        private static void schedule_Timer()
        {
            CultureInfo provider = new CultureInfo("hi-IN");
            DateTime dateTime1 = Convert.ToDateTime((object)DateTime.Now, (IFormatProvider)provider);
            DateTime dateTime2 = Convert.ToDateTime((object)new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day, 4, 30, 0, 0), (IFormatProvider)provider);
            if (dateTime1 > dateTime2)
                dateTime2 = dateTime2.AddDays(1.0);
            Scheduler.timer = new Timer((dateTime2 - dateTime1).TotalMilliseconds);
            Scheduler.timer.Elapsed += new ElapsedEventHandler(Scheduler.timer_Elapsed);
            Scheduler.timer.Start();
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Scheduler.timer.Stop();
            CultureInfo cult = new CultureInfo("hi-IN");
            SLAManager.getSLASRRequests();
            SLAManager.getSLABreakFixRequests().AddRange((IEnumerable<Laptop_Master>)SLAManager.getSLAEmployeeMovingOnshoreRequests());
            TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
            List<string> toMailIDs1 = new List<string>();
            List<string> toMailIDs2 = new List<string>();
            List<string> toMailIDs3 = new List<string>();
            var upcoming = new List<Onboarding_Master>();
            foreach (var entry in ticketingToolDbEntities.Onboarding_Master.Where(x=> x.isDeleted == "No").ToList())
            {
                if (entry.actualJoiningDate!=null)
                {
                    var jDate = Convert.ToDateTime(entry.actualJoiningDate, cult);
                    var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                    if ((diff >= 0) && (diff <= 7))
                    {
                        upcoming.Add(entry);
                    }
                }
                else
                {
                    var jDate = Convert.ToDateTime(entry.expectedJoiningDate, cult);
                    var diff = (jDate - Convert.ToDateTime(DateTime.Now, cult)).Days;
                    if ((diff >= 0) && (diff <= 7))
                    {
                        upcoming.Add(entry);
                    }
                }
            }
            foreach (Mail_Master mailMaster in ticketingToolDbEntities.Mail_Master.ToList<Mail_Master>())
            {
                foreach (char ch in mailMaster.type)
                {
                    switch (ch)
                    {
                        case 'L':
                            toMailIDs2.Add(mailMaster.email);
                            break;
                        case 'S':
                            toMailIDs1.Add(mailMaster.email);
                            break;
                        case 'O':
                            toMailIDs3.Add(mailMaster.email);
                            break;
                    }
                }
            }
            Mail.SendPendingMail(ticketingToolDbEntities.SR_Request.Where<SR_Request>((Expression<Func<SR_Request, bool>>)(x => x.isDeleted == (bool?)false && x.status == "Pending")).ToList<SR_Request>(), toMailIDs1, cult);
            Mail.SendLaptopPendingMail(ticketingToolDbEntities.Laptop_Master.Where<Laptop_Master>((Expression<Func<Laptop_Master, bool>>)(x => x.isDeleted == "No" && x.status == "Pending")).ToList<Laptop_Master>(), toMailIDs2, cult);
            Mail.SendOnboardingPendingMail(upcoming, toMailIDs3, cult);
            Scheduler.schedule_Timer();
        }
    }
}
