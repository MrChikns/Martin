namespace HotelGarage.Models
{
    public class StateOfPlace
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static readonly byte Free = 1;
        public static readonly byte Occupied = 2;
        public static readonly byte Reserved = 3;
        public static readonly byte Employee = 4;
    }
}