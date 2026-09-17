namespace CQRS_RentACar.CQRSPattern.Queries.BookingQueries
{
    public class GetBookingByIdQuery
    {
        public int BookingId { get; set; }

        public GetBookingByIdQuery(int bookingId)
        {
            BookingId = bookingId;
        }
    }
}
