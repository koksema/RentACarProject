using CQRS_RentACar.Models;
using System.Collections.Generic;

namespace CQRS_RentACar.CQRSPattern.Results.FuelResults
{
    public class GetFuelPricesQueryResult
    {
        public List<FuelPriceModel> FuelPrices { get; set; }
            = new List<FuelPriceModel>();

        public string Province { get; set; }

        public string Date { get; set; }
    }
}