using System.Text.Json;

namespace HotelAvailability
{
    public class App
    {

        private List<Booking> bookingList;
        private List<Hotel> hotelList;

        public App()
        {
            bookingList = new List<Booking>();
            hotelList = new List<Hotel>();
        }

        public App(List<Booking> bookingList, List<Hotel> hotelList)
        {
            this.bookingList = bookingList;
            this.hotelList = hotelList;
        }

        private void readBookings(string filename)
        {
            string jsonString = File.ReadAllText(filename);
            var options = new JsonSerializerOptions();
            options.Converters.Add(new CustomDateOnlyConverter());
            List<Booking> bookings = JsonSerializer.Deserialize<List<Booking>>(jsonString, options)!;

            bookingList.AddRange(bookings);
        }

        private void readHotels(string filename)
        {
            string jsonString = File.ReadAllText(filename);
            List<Hotel> hotels = JsonSerializer.Deserialize<List<Hotel>>(jsonString)!;
            hotelList.AddRange(hotels);
        }

        /// <summary>
        /// Counts number of already existing bookings that overlap with the tested booking
        /// </summary>
        public int countBookingConflicts(Booking booking)
        {
            return bookingList.Where(b => (b.hotelId == booking.hotelId && b.roomType == booking.roomType)).
                Where(b => b.arrival.CompareTo(booking.departure) <= 0).
                Where(b => b.departure.CompareTo(booking.arrival) >= 0).Count();
        }

        /// <summary>
        /// Counts number of rooms with matching room type and hotel id for the given booking
        /// </summary>
        public int countMatchingRooms(Booking booking)
        {
            Hotel hotel = hotelList.Find(hotel => hotel.id == booking.hotelId);
            if (hotel is null) return -1;
            return hotel.rooms.Where(room => room.roomType == booking.roomType).Count();
        }

        /// <summary>
        /// Creates Booking type object from text input
        /// </summary>
        public Booking getBookingFromInput(string input)
        {
            int pom = input.IndexOf('(');
            string[] data = input.Substring(pom + 1, input.Length - pom - 2).Split(',').
                Select(s => s.Trim()).ToArray();

            Booking booking = new Booking();
            booking.hotelId = data[0];
            if (data[1].Contains('-'))
            {
                string[] dates = data[1].Split('-').Select(s => s.Trim()).ToArray();
                booking.arrival = DateOnly.ParseExact(dates[0], Constants.DATE_FORMAT);
                booking.departure = DateOnly.ParseExact(dates[1], Constants.DATE_FORMAT);
            }
            else
            {
                booking.arrival = DateOnly.ParseExact(data[1], Constants.DATE_FORMAT);
                booking.departure = DateOnly.ParseExact(data[1], Constants.DATE_FORMAT);
            }
            booking.roomType = data[2];
            return booking;
        }

        public string proccessInput(string input)
        {
            if (input.StartsWith(Constants.CHECK_AVAILABILITY+"(") && input.EndsWith(')'))
            {
                Booking booking = null;
                try
                {
                    booking = getBookingFromInput(input);
                }
                catch (Exception ex)
                {
                    return Constants.INVALID_INPUT;
                }

                int matchingRooms = countMatchingRooms(booking);
                if (matchingRooms == 0)
                {
                    return Constants.ROOM_TYPE_UNAVAILABLE;
                }
                else if(matchingRooms == -1)
                {
                    return Constants.HOTEL_UNAVAILABLE;
                }
                int conlictingBookingsCount = countBookingConflicts(booking);
                int result = matchingRooms - conlictingBookingsCount;
                return result.ToString();
            }
            return Constants.INVALID_INPUT;
        }

        static void Main(string[] args)
        {
            var test = DateOnly.ParseExact("20200509", "yyyyMMdd");
            
            
            App app = new App();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].StartsWith('-'))
                {
                    string option = args[i];
                    while(!args[i + 1].StartsWith('-'))
                    {
                        i++;
                        if (option == "--hotels")
                        {
                            app.readHotels(args[i]);
                            //"D:\\praca\\challange\\HotelAvailability\\HotelAvailability\\" + 
                        }
                        else if (option == "--bookings" )
                        {
                            app.readBookings(args[i]);
                        }
                        if (i == args.Length-1) break;
                        
                    }
                    
                }
                
            }

            while (true)
            {
                string? input = Console.ReadLine();
                if (input == "") break;
                string response = app.proccessInput(input);
                Console.WriteLine(response);
            }

        }
    }

}

