namespace CQRS_RentACar.CQRSPattern.Queries.ServicesQueries
{
    public class GetServicesByIdQuery
    {
        public int ServicesId { get; set; }

        public GetServicesByIdQuery(int serviceId)
        {
            ServicesId = serviceId;
        }
    }
}
