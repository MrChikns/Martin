namespace HotelGarage.Core.Models
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

    public enum StateOfPlaceEnum
    {
        Free = 1,
        Occupied = 2,
        Reserved = 3,
        Employee = 4
    }
}