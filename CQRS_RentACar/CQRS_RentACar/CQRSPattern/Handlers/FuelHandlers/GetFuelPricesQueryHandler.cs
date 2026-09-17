//using CQRS_RentACar.CQRSPattern.Queries.FuelQueries;
//using CQRS_RentACar.Models;
//using System;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace CQRS_RentACar.CQRSPattern.Handlers.FuelHandlers
//{
//    public class GetFuelPricesQueryHandler
//    {
//        private readonly IHttpClientFactory _httpClientFactory;

//        public GetFuelPricesQueryHandler(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }

//        // Handle metodu parametre olarak Query nesnesini alır
//        public async Task<FuelPriceModel> Handle(GetFuelPricesQuery query)
//        {
//            var client = _httpClientFactory.CreateClient();

//            var request = new HttpRequestMessage
//            {
//                Method = HttpMethod.Get,
//                RequestUri = new Uri("https://turkey-fuel-prices.p.rapidapi.com/v1/series/history"),
//                Headers =
//                {
//                   { "x-rapidapi-key", "Fuel" },
//        { "x-rapidapi-host", "turkey-fuel-prices.p.rapidapi.com" },
//                },
//            };

//            try
//            {
//                using (var response = await client.SendAsync(request))
//                {
//                    if (response.IsSuccessStatusCode)
//                    {
//                        var body = await response.Content.ReadAsStringAsync();
//                        using var doc = JsonDocument.Parse(body);
//                        var root = doc.RootElement;

//                        return new FuelPriceModel
//                        {
//                            Gasoline = root.TryGetProperty("gasoline", out var g) ? g.GetString() : "43.50 ₺",
//                            Diesel = root.TryGetProperty("diesel", out var d) ? d.GetString() : "44.10 ₺",
//                            Lpg = root.TryGetProperty("lpg", out var l) ? l.GetString() : "22.80 ₺"
//                        };
//                    }
//                }
//            }
//            catch
//            {
//                // API hatası durumunda fallback değerler
//            }

//            return new FuelPriceModel
//            {
//                Gasoline = "43.50 ₺",
//                Diesel = "44.10 ₺",
//                Lpg = "22.80 ₺"
//            };
//        }
//    }
//}
using CQRS_RentACar.CQRSPattern.Queries.FuelQueries;
using CQRS_RentACar.CQRSPattern.Results.FuelResults;
using CQRS_RentACar.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS_RentACar.CQRSPattern.Handlers.FuelHandlers
{
    public class GetFuelPricesQueryHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GetFuelPricesQueryHandler(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<GetFuelPricesQueryResult> Handle(
            GetFuelPricesQuery query)
        {
            var apiKey =
                _configuration["RapidApi:FuelApiKey"];

            var apiHost =
                _configuration["RapidApi:FuelApiHost"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new Exception(
                    "RapidApi:FuelApiKey bulunamadı."
                );
            }

            if (string.IsNullOrWhiteSpace(apiHost))
            {
                throw new Exception(
                    "RapidApi:FuelApiHost bulunamadı."
                );
            }

            var client =
                _httpClientFactory.CreateClient();

            // product parametresini göndermiyoruz.
            // Böylece İstanbul için desteklenen
            // bütün ürünler tek istekte geliyor.
            var url =
                "https://turkey-fuel-prices.p.rapidapi.com" +
                "/v1/tr/fuel/prices" +
                "?includeDistricts=false" +
                "&province=34";

            using var request =
                new HttpRequestMessage(
                    HttpMethod.Get,
                    url
                );

            request.Headers.Add(
                "x-rapidapi-key",
                apiKey
            );

            request.Headers.Add(
                "x-rapidapi-host",
                apiHost
            );

            using var response =
                await client.SendAsync(request);

            var json =
                await response.Content
                    .ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(
                    $"Yakıt API Hatası: " +
                    $"HTTP {(int)response.StatusCode} " +
                    $"{response.StatusCode}"
                );
            }

            using var document =
                JsonDocument.Parse(json);

            var root =
                document.RootElement;

            // ok kontrolü
            if (!root.TryGetProperty(
                    "ok",
                    out var okElement) ||
                !okElement.GetBoolean())
            {
                throw new Exception(
                    "Yakıt API başarılı cevap döndürmedi."
                );
            }

            // data
            if (!root.TryGetProperty(
                "data",
                out var data))
            {
                throw new Exception(
                    "API cevabında data bulunamadı."
                );
            }

            var result =
                new GetFuelPricesQueryResult();

            // İl bilgisi
            if (data.TryGetProperty(
                "province",
                out var province))
            {
                if (province.TryGetProperty(
                    "name",
                    out var provinceName))
                {
                    result.Province =
                        provinceName.GetString();
                }
            }

            // Tarih
            if (data.TryGetProperty(
                "asOf",
                out var date))
            {
                result.Date =
                    date.GetString();
            }

            // Fiyat listesi
            if (!data.TryGetProperty(
                "prices",
                out var prices))
            {
                throw new Exception(
                    "API cevabında prices bulunamadı."
                );
            }

            if (prices.ValueKind !=
                JsonValueKind.Array)
            {
                throw new Exception(
                    "prices alanı liste formatında değil."
                );
            }

            var fuelPrices =
                new List<FuelPriceModel>();

            foreach (var item
                     in prices.EnumerateArray())
            {
                if (!item.TryGetProperty(
                    "product",
                    out var productElement))
                {
                    continue;
                }

                if (!item.TryGetProperty(
                    "price",
                    out var priceElement))
                {
                    continue;
                }

                var product =
                    productElement.GetString();

                decimal price;

                if (!priceElement.TryGetDecimal(
                    out price))
                {
                    continue;
                }

                if (string.Equals(
                    product,
                    "benzin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    fuelPrices.Add(
                        new FuelPriceModel
                        {
                            FuelType = "Gasoline",
                            Price = price
                        }
                    );
                }
                else if (string.Equals(
                    product,
                    "motorin",
                    StringComparison.OrdinalIgnoreCase))
                {
                    fuelPrices.Add(
                        new FuelPriceModel
                        {
                            FuelType = "Diesel",
                            Price = price
                        }
                    );
                }
            }

            result.FuelPrices =
                fuelPrices;

            return result;
        }
    }
}