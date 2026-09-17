namespace CQRS_RentACar.CQRSPattern.Results.BookingResults
{
    public class GetBookingByIdQueryResult
    {
        public int BookingId { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public int CarsId { get; set; }
        public string CarModel { get; set; }
        public string CarBrand { get; set; }
    }
}
