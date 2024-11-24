

namespace HotelAvailability
{
    //public class Booking
    //{
    //    public Booking(string hotelId, string roomType, string arrival, string departure)
    //    {
    //        this.hotelId = hotelId;
    //        this.roomType = roomType;
    //        this.arrival = arrival;
    //        this.departure = departure;
    //    }

    //    public Booking() { }

    //    public string hotelId { get; set; }
    //    public string roomType { get; set; }
    //    public string arrival { get; set; }
    //    public string departure { get; set; }
    //}

    public class Booking
    {
        public Booking(string hotelId, string roomType, DateOnly arrival, DateOnly departure)
        {
            this.hotelId = hotelId;
            this.roomType = roomType;
            this.arrival = arrival;
            this.departure = departure;
        }

        public Booking() { }

        public string hotelId { get; set; }
        public string roomType { get; set; }
        public DateOnly arrival { get; set; }
        public DateOnly departure { get; set; }
    }
}
