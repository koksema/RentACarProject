namespace CQRS_RentACar.CQRSPattern.Results.StatisticResults
{
    public class GetStatisticsQueryResult
    {
        public int TotalCarCount { get; set; }           
        public int TotalLocationCount { get; set; }      
        public string MostExpensiveCar { get; set; }    
        public string MostPopularLocation { get; set; } 
    }
}
