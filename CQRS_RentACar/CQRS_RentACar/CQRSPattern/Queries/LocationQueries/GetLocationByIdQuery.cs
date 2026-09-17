namespace CQRS_RentACar.CQRSPattern.Queries.LocationQueries
{
    public class GetLocationByIdQuery
    {
       public int LocationId { get; set; }

        public GetLocationByIdQuery(int locationId)
        {
            LocationId = locationId;
        }
    }
}
