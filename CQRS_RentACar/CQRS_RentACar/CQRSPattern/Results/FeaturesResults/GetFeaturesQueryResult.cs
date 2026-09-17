namespace CQRS_RentACar.CQRSPattern.Results.FeaturesResults
{
    public class GetFeaturesQueryResult
    {
        public int FeaturesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string IconTitle { get; set; }
        public string IconDescription { get; set; }
        public string ImgUrl { get; set; }
    }
}
