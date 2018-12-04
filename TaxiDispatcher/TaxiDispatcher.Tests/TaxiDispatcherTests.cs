using NUnit.Framework;
using System;
using System.Linq;
using TaxiDispatcher.App;

namespace TaxiDispatcher.Tests
{
    public class TaxiDispatcherTests
    {
        [Test]
        public void OrderRideTaxiDriverId()
        {
            var scheduler = new Scheduler();
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));

            Assert.AreEqual(ride.TaxiDriverId, 2);
        }

        [Test]
        public void OrderRidePrice()
        {
            var scheduler = new Scheduler();
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));

            Assert.AreEqual(ride.Price, 100);
        }

        [Test]
        public void AcceptRideNewLocation()
        {
            var scheduler = new Scheduler();
            var ride = scheduler.OrderRide(5, 0, Constants.City, new DateTime(2018, 1, 1, 23, 0, 0));
            scheduler.AcceptRide(ride);

            Assert.AreEqual(scheduler.GetTaxi(2).Location, 0);
        }

        [Test]
        public void OrderRideException()
        {
            var scheduler = new Scheduler();

            Exception e = Assert.Throws<Exception>(delegate
            {
                scheduler.OrderRide(55, 0, Constants.InterCity, new DateTime(2018, 1, 1, 23, 0, 0));
            });
            Assert.That(e.Message, Is.EqualTo("There are no available taxi vehicles!"));
        }
    }
}
