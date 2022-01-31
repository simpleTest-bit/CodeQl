using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using Ticketing_Dashboard.Models;

namespace Ticketing_Dashboard.Utility
{
    public class SLAManager
    {

        public static List<SR_Request> getSLASRRequests()
        {
            CultureInfo provider = new CultureInfo("hi-IN");
            var currentDate = Convert.ToDateTime(DateTime.Now, provider);
            TicketingToolDBEntities db = new TicketingToolDBEntities();
            var sr = db.SR_Request.Where(x => x.isDeleted == false && currentDate >= x.slaDate).ToList();
            return sr;
        }

        public static List<Laptop_Master> getSLABreakFixRequests()
        {
            CultureInfo provider = new CultureInfo("hi-IN");
            var currentDate = Convert.ToDateTime(DateTime.Now, provider);
            TicketingToolDBEntities db = new TicketingToolDBEntities();
            var laptop = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.requestType == "Break Fix" && currentDate >= x.slaDate).ToList();
            return laptop;
        }

        public static List<Laptop_Master> getSLAEmployeeMovingOnshoreRequests()
        {
            CultureInfo provider = new CultureInfo("hi-IN");
            var currentDate = Convert.ToDateTime(DateTime.Now, provider);
            TicketingToolDBEntities db = new TicketingToolDBEntities();
            var laptop = db.Laptop_Master.Where(x => x.isDeleted == "No" && x.requestType == "Moving To Onshore / Offshore" && currentDate >= x.slaDate).ToList();
            return laptop;
        }

        public static bool CheckLaptopSLAExceed(Laptop_Master laptop)
        {
            CultureInfo cult = new CultureInfo("hi-IN");
            var currentDate = Convert.ToDateTime(DateTime.Now, cult);
            return currentDate >= Convert.ToDateTime(laptop.slaDate, cult);
        }

        public static bool CheckSRSLAExceed(SR_Request sr)
        {
            CultureInfo cult = new CultureInfo("hi-IN");
            var currentDate = Convert.ToDateTime(DateTime.Now, cult);
            return currentDate >= Convert.ToDateTime(sr.slaDate, cult);
        }
    }
}
