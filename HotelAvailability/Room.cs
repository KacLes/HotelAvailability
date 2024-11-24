namespace HotelAvailability
{
    public class Room
    {
        public Room(string roomId, string roomType)
        {
            this.roomId = roomId;
            this.roomType = roomType;
        }

        public string roomId { get; set; }
        public string roomType { get; set; }

    }
}
