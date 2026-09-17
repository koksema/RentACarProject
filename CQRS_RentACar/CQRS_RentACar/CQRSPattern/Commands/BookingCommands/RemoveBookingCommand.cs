namespace CQRS_RentACar.CQRSPattern.Commands.BookingCommands
{
    public class RemoveBookingCommand
    {
        public int BookingId { get; set; }

        public RemoveBookingCommand(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}
