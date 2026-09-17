namespace CQRS_RentACar.CQRSPattern.Results.ServicesResults
{
    public class GetServicesQueryResult
    {
        public int ServicesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
    }
}
