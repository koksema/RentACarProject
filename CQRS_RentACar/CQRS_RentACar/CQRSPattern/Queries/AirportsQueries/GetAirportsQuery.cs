namespace CQRS_RentACar.CQRSPattern.Queries.AirportsQueries
{
    public class GetAirportsQuery
    {
        public string CountryCode { get; set; }

        public GetAirportsQuery(string countryCode)
        {
            CountryCode = countryCode;
        }
    }
}
