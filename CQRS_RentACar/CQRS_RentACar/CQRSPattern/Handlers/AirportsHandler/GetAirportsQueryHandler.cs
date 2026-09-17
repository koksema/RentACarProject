using CQRS_RentACar.CQRSPattern.Results.AirportsResults;
using CQRS_RentACar.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq; // LINQ metotları (Any, Select) için şart
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS_RentACar.CQRSPattern.Handlers.AirportsHandler
{
    public class GetAirportsQueryHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public GetAirportsQueryHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<List<GetAirportsQueryResult>> Handle()
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Önce Türkiye havalimanlarını çekmeyi dene
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://airports-runways-and-airlines-worldwide-api.p.rapidapi.com/airports/list?country=turkey"),
                Headers =
                {
                    { "x-rapidapi-key", "8dfe98e13dmsh31597df2f233e3ap117484jsn77141dd9adeb" },
                    { "x-rapidapi-host", "airports-runways-and-airlines-worldwide-api.p.rapidapi.com" },
                }
            };

            using (var response = await client.SendAsync(request))
            {
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiData = JsonSerializer.Deserialize<GetAirportsViewModel.Rootobject>(json);

                    if (apiData != null && apiData.data != null && apiData.data.Any())
                    {
                        return apiData.data.Select(x => new GetAirportsQueryResult
                        {
                            IataCode = x.iata,
                            Name = x.name,
                            Country = x.country
                        }).ToList();
                    }
                }
            }

            return new List<GetAirportsQueryResult>();
        }
    }
}


//8dfe98e13dmsh31597df2f233e3ap117484jsn77141dd9adeb
//airports-runways-and-airlines-worldwide-api.p.rapidapi.com