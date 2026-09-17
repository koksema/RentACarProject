namespace CQRS_RentACar.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }

        public int CarsId { get; set; }
        public Cars Car { get; set; }
    }
}
