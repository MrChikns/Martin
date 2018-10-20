namespace HotelGarage.Models
{
    public class StateOfReservation
    {        
        public int Id { get; set; }
        public string State { get; set; }


        public static readonly byte Reserved = 0;
        public static readonly byte Inhouse = 1;
        public static readonly byte Departed = 2;
        public static readonly byte Cancelled = 3;
    }
}