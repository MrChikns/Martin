using HotelGarage.Helpers;

namespace HotelGarage.Core.Dtos
{
    public class BootBoxDataDto
    {
        public BootBoxDataDto()
        {
            Departure = Labels.Empty;
            IsRegistered = Labels.Empty;
            Arrival = Labels.Empty;
            GuestName = Labels.Empty;
            RoomNumber = Labels.Empty;
            CarModel = Labels.Empty;
            PricePerNight = Labels.Empty;
            LicensePlate = Labels.Empty;
            IsEmployee = Labels.Empty;
            Note = Labels.Empty;
        }

        public string Departure { get; set; }
        public string Arrival { get; set; }
        public string GuestName { get; set; }
        public string RoomNumber { get; set; }
        public string CarModel { get; set; }
        public string PricePerNight { get; set; }
        public string LicensePlate { get; set; }
        public string IsEmployee { get; set; }
        public string Note { get; set; }
        public string IsRegistered { get; set; }
    }
}