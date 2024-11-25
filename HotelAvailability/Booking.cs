

namespace HotelAvailability
{
    
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
