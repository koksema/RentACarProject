namespace CQRS_RentACar.CQRSPattern.Results.AboutResults
{
    public class GetAboutByIdQueryResult
    {
        public int AboutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
