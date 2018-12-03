using System;
using System.Collections.Generic;
using System.Linq;

namespace TaxiDispatcher.App
{
    public class Scheduler
    {
        protected List<Taxi> Taxis = new List<Taxi>();

        public Scheduler()
        {
            Taxis.Add(new Taxi()
            {
                TaxiDriverId = 1,
                TaxiDriverName = "Predrag",
                TaxiCompany = "Naxi",
                Location = 1
            });
            Taxis.Add(new Taxi()
            {
                TaxiDriverId = 2,
                TaxiDriverName = "Nenad",
                TaxiCompany = "Naxi",
                Location = 4
            });
            Taxis.Add(new Taxi()
            {
                TaxiDriverId = 3,
                TaxiDriverName = "Dragan",
                TaxiCompany = "Alfa",
                Location = 6
            });
            Taxis.Add(new Taxi()
            {
                TaxiDriverId = 4,
                TaxiDriverName = "Goran",
                TaxiCompany = "Gold",
                Location = 7
            });
        }
        public Ride OrderRide(int locationFrom, int locationTo, int rideType, DateTime time)
        {
            #region FindingTheBestVehicle 

            if (locationFrom == locationTo)
            {
                throw new Exception("Cannot order ride to the same location!");
            }

            Taxi minTaxi = Taxis.OrderBy(x => Math.Abs(x.Location - locationFrom)).FirstOrDefault();
            if (minTaxi == null)
            {
                throw new Exception("There are no available taxi vehicles!");
            }

            if (Math.Abs(minTaxi.Location - locationFrom) > 15)
                throw new Exception("There are no available taxi vehicles!");

            #endregion

            #region CreatingRide

            Ride ride = new Ride
            {
                TaxiDriverId = minTaxi.TaxiDriverId,
                LocationFrom = locationFrom,
                LocationTo = locationTo,
                TaxiDriverName = minTaxi.TaxiDriverName
            };

            #endregion

            #region CalculatingPrice

            var distance = Math.Abs(locationFrom - locationTo);
            switch (minTaxi.TaxiCompany)
            {
                case "Naxi":
                {
                    ride.Price = 10 * distance;
                    break;
                }
                case "Alfa":
                {
                    ride.Price = 15 * distance;
                    break;
                }
                case "Gold":
                {
                    ride.Price = 13 * distance;
                    break;
                }
                default:
                {
                    throw new Exception("Ilegal company");
                }
            }

            if (rideType == Constants.InterCity)
            {
                ride.Price *= 2;
            }

            if (time.Hour < 6 || time.Hour > 22)
            {
                ride.Price *= 2;
            }

            #endregion

            Console.WriteLine("Ride ordered, price: " + ride.Price);
            return ride;
        }

        public void AcceptRide(Ride ride)
        {
            InMemoryRideDataBase.SaveRide(ride);
            Taxis.FirstOrDefault(x => x.TaxiDriverId == ride.TaxiDriverId).Location = ride.LocationTo;
            
            Console.WriteLine("Ride accepted, waiting for driver: " + ride.TaxiDriverName);
        }

        public List<Ride> GetRideList(int driverId)
        {
            return InMemoryRideDataBase.GetRides().Where(x => x.TaxiDriverId == driverId).ToList();
        }

        public class Taxi
        {
            public int TaxiDriverId { get; set; }
            public string TaxiDriverName { get; set; }
            public string TaxiCompany { get; set; }
            public int Location { get; set; }
        }

        public class Ride
        {
            public int RideId { get; set; }
            public int LocationFrom { get; set; }
            public int LocationTo { get; set; }
            public int TaxiDriverId { get; set; }
            public string TaxiDriverName { get; set; }
            public int Price { get; set; }
        }
    }
}
