using System.Collections.Generic;
using System.Linq;
using Ticketing_Dashboard.Models;

namespace Ticketing_Dashboard.Utility
{
    public static class Location
    {
        public static string SetLocation(string location)
        {
            TicketingToolDBEntities ticketingToolDbEntities = new TicketingToolDBEntities();
            List<Location_Master> list = ticketingToolDbEntities.Location_Master.ToList<Location_Master>();
            foreach (Location_Master locationMaster in list)
            {
                if (locationMaster.city.ToLower().Trim() == location.ToLower().Trim())
                    return locationMaster.city;
            }
            Location_Master entity = new Location_Master();
            entity.id = list.Count + 1;
            entity.city = location;
            ticketingToolDbEntities.Location_Master.Add(entity);
            ticketingToolDbEntities.SaveChanges();
            return entity.city;
        }
    }
}
