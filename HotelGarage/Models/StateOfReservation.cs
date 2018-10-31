namespace HotelGarage.Models
{
    public class StateOfReservation
    {        
        public int Id { get; set; }
        public string State { get; set; }

        public static readonly byte Reserved = 1;
        public static readonly byte Inhouse = 2;
        public static readonly byte Departed = 3;
        public static readonly byte Cancelled = 4;
    }
}