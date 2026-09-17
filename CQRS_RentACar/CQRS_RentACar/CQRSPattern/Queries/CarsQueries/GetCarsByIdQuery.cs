namespace CQRS_RentACar.CQRSPattern.Queries.CarsQueries
{
    public class GetCarsByIdQuery
    {
        public int CarsId { get; set; }

        public GetCarsByIdQuery(int carsId)
        {
            CarsId = carsId;
        }
    }
}
