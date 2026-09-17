namespace CQRS_RentACar.CQRSPattern.Queries.FeaturesQueries
{
    public class GetFeaturesByIdQuery
    {
        public int FeaturesId { get; set; }

        public GetFeaturesByIdQuery(int featureId)
        {
            FeaturesId = featureId;
        }
    }
}
