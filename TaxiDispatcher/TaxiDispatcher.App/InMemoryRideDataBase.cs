using System.Collections.Generic;
using System.Linq;
using static TaxiDispatcher.App.Scheduler;

namespace TaxiDispatcher.App
{
    public static class InMemoryRideDataBase
    {
        private static readonly List<Ride> Rides = new List<Ride>();

        public static void SaveRide(Ride ride)
        {
            int maxId = Rides.Count == 0 ? 0 : Rides.Max(x => x.RideId);

            ride.RideId = maxId + 1;
            Rides.Add(ride);
        }

        public static List<Ride> GetRides()
        {
            return Rides;
        }
    }
}
