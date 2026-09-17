namespace CQRS_RentACar.CQRSPattern.Queries.BookingQueries
{
    public class GetAvailableCarsQuery
    {
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public string PickUpLocation { get; set; }

        public GetAvailableCarsQuery(DateTime pickUpDate, DateTime dropOffDate, string pickUpLocation)
        {
            PickUpDate = pickUpDate;
            DropOffDate = dropOffDate;
            PickUpLocation = pickUpLocation;
        }
    }
}
