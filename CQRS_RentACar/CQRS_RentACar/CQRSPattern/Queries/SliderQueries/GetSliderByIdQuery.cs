namespace CQRS_RentACar.CQRSPattern.Queries.SliderQueries
{
    public class GetSliderByIdQuery
    {
        public int SliderId { get; set; }

        public GetSliderByIdQuery(int sliderId)
        {
            SliderId = sliderId;
        }
    }
}
