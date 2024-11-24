using NUnit.Framework;
using HotelAvailability;
using System.Collections.Generic;
using System;

namespace HotelAvailabilityTest
{
    [TestFixture]
    public class HotelAvailabilityTest
    {
        [Test]
        public void TestGetBookingFromInput()
        {
            App app = new App();
            
            Booking booking = app.getBookingFromInput(Constants.CHECK_AVAILABILITY + "(H1,20240901 - 20240903, DBL)");
            
            Assert.AreEqual(booking.hotelId, "H1");
            Assert.AreEqual(booking.arrival, DateOnly.ParseExact("20240901",Constants.DATE_FORMAT));
            Assert.AreEqual(booking.departure, DateOnly.ParseExact("20240903",Constants.DATE_FORMAT));
            Assert.AreEqual(booking.roomType, "DBL");
        }
        [Test]
        public void TestGetBookingFromInputSingleDayStay()
        {
            App app = new App();
            
            Booking booking = app.getBookingFromInput(Constants.CHECK_AVAILABILITY + "(H1,20240901, DBL)");
            
            Assert.AreEqual(booking.hotelId, "H1");
            Assert.AreEqual(booking.arrival, DateOnly.ParseExact("20240901", Constants.DATE_FORMAT));
            Assert.AreEqual(booking.departure, DateOnly.ParseExact("20240901", Constants.DATE_FORMAT));
            Assert.AreEqual(booking.roomType, "DBL");
        }
        [Test]
        public void TestProccessInputInvalidInput()
        {
            App app = new App();
            
            string response = app.proccessInput("Invalid");
            
            Assert.AreEqual(response, Constants.INVALID_INPUT);
        }
        [Test]
        public void TestProccessInputNoHotel()
        {
            App app = new App();
            
            string response = app.proccessInput(Constants.CHECK_AVAILABILITY+"(H1,20240901-20240903,DBL)");
            
            Assert.AreEqual(response, Constants.HOTEL_UNAVAILABLE);
        }
        [Test]
        public void TestProccessInputNoRoomtype()
        {
            List<Hotel> hotelList = new List<Hotel>();
            hotelList.Add(new Hotel("H1", Array.Empty<Room>()));           
            App app = new App(new List<Booking>(),hotelList);

            string response = app.proccessInput(Constants.CHECK_AVAILABILITY + "(H1,20240901-20240903,DBL)");
            
            Assert.AreEqual(response, Constants.ROOM_TYPE_UNAVAILABLE);
        }
        [Test]
        public void TestProccessInput()
        {
            List<Hotel> hotelList = new List<Hotel>();
            Room[] rooms = new Room[] { new Room("101", "SGL"), new Room("102", "DBL") };
            hotelList.Add(new Hotel("H1", rooms));
            List<Booking> bookingList = new List<Booking>();
            DateOnly arrival = DateOnly.ParseExact("20240902", Constants.DATE_FORMAT);
            DateOnly departure = DateOnly.ParseExact("20240904", Constants.DATE_FORMAT);
            bookingList.Add(new Booking("H1", "SGL", arrival,departure));

            App app = new App(bookingList, hotelList);

            string response = app.proccessInput(Constants.CHECK_AVAILABILITY + "(H1,20240901-20240903,SGL)");

            Assert.AreEqual(response, "0");
        }

        [Test]
        public void TestProccessInputOverbooking()
        {
            List<Hotel> hotelList = new List<Hotel>();
            Room[] rooms = new Room[] { new Room("101", "SGL"), new Room("102", "DBL") };
            hotelList.Add(new Hotel("H1", rooms));
            List<Booking> bookingList = new List<Booking>();
            DateOnly arrival = DateOnly.ParseExact("20240902", Constants.DATE_FORMAT);
            DateOnly departure = DateOnly.ParseExact("20240904", Constants.DATE_FORMAT);
            bookingList.Add(new Booking("H1", "SGL", arrival, departure));
            bookingList.Add(new Booking("H1", "SGL", arrival, departure));

            App app = new App(bookingList, hotelList);

            string response = app.proccessInput(Constants.CHECK_AVAILABILITY + "(H1,20240901-20240903,SGL)");

            Assert.AreEqual(response, "-1");
        }

        [Test]
        public void TestCountBookingConflicts()
        {
            List<Booking> bookingList = new List<Booking>();
            DateOnly arrival = DateOnly.ParseExact("20240902", Constants.DATE_FORMAT);
            DateOnly departure = DateOnly.ParseExact("20240904", Constants.DATE_FORMAT);
            bookingList.Add(new Booking("H1", "SGL", arrival , departure));
            arrival = DateOnly.ParseExact("20240901", Constants.DATE_FORMAT);
            departure = DateOnly.ParseExact("20240907", Constants.DATE_FORMAT);
            bookingList.Add(new Booking("H1", "SGL", arrival, departure));
            bookingList.Add(new Booking("H2", "SGL", arrival, departure));
            bookingList.Add(new Booking("H1", "DBL", arrival, departure));
            arrival = DateOnly.ParseExact("20240903", Constants.DATE_FORMAT);
            departure = DateOnly.ParseExact("20240905", Constants.DATE_FORMAT);
            Booking testBooking = new Booking("H1", "SGL", arrival , departure);

            App app = new App(bookingList, new List<Hotel>());

            int response = app.countBookingConflicts(testBooking);

            Assert.AreEqual(response, 2);

        }

        [Test]
        public void TestCountMatchingRooms()
        {
            List<Hotel> hotelList = new List<Hotel>();
            Room[] rooms = new Room[] { new Room("101", "SGL"), new Room("102", "SGL"), new Room("103", "DBL") };
            hotelList.Add(new Hotel("H1", rooms));
            hotelList.Add(new Hotel("H2", rooms));
            DateOnly arrival = DateOnly.ParseExact("20240903", Constants.DATE_FORMAT);
            DateOnly departure = DateOnly.ParseExact("20240905", Constants.DATE_FORMAT);
            Booking testBooking = new Booking("H1", "SGL", arrival, departure);
            App app = new App(new List<Booking>(), hotelList);

            int response = app.countMatchingRooms(testBooking);

            Assert.AreEqual(response, 2);
        }
    }

}