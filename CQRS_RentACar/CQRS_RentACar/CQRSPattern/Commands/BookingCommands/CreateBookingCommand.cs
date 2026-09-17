namespace CQRS_RentACar.CQRSPattern.Commands.BookingCommands
{
    public class CreateBookingCommand
    {
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }

        public int CarId { get; set; }
    }
}
