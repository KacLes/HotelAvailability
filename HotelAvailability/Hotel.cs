

namespace HotelAvailability
{
    public class Hotel
    {
        public Hotel(string id, Room[] rooms)
        {
            this.id = id;
            this.rooms = rooms;
        }

        public string id { get; set; }
        public Room[] rooms { get; set; }
    }
}
