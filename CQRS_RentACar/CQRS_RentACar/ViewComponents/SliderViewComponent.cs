using CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers;
using CQRS_RentACar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CQRS_RentACar.ViewComponents
{
    public class SliderViewComponent : ViewComponent
    {
        private readonly GetSliderQueryHandler _handler;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SliderViewComponent(
            GetSliderQueryHandler handler,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _handler = handler;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sliderValues =
                await _handler.Handle();


            ViewBag.AirportList =
                new List<SelectListItem>();

            ViewBag.AirportCoordinatesJson =
                "{}";

            ViewBag.AirportError =
                null;


            try
            {
                var apiKey =
                    _configuration[
                        "RapidApi:AirportApiKey"
                    ];


                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    ViewBag.AirportError =
                        "Airport API key bulunamadı.";

                    return View(sliderValues);
                }


                const string apiHost =
                    "airports-runways-and-airlines-worldwide-api.p.rapidapi.com";


                var client =
                    _httpClientFactory.CreateClient();


                var allAirports =
                    new List<GetAirportsViewModel.Datum>();


                // ============================================
                // TÜM SAYFALARI TEK TEK ÇEK
                // ============================================

                int page = 1;

                while (true)
                {
                    var pageUrl =
                        $"https://{apiHost}" +
                        $"/airports/list" +
                        $"?country=turkey" +
                        $"&page={page}";


                    var pageResult =
                        await GetAirportPage(
                            client,
                            pageUrl,
                            apiKey,
                            apiHost
                        );


                    // API isteği başarısızsa
                    if (pageResult == null)
                    {
                        Console.WriteLine(
                            $"SAYFA {page} ALINAMADI."
                        );

                        break;
                    }


                    // Data boşsa artık sayfa yok
                    if (
                        pageResult.data == null
                        ||
                        pageResult.data.Length == 0
                    )
                    {
                        Console.WriteLine(
                            $"SAYFA {page} BOŞ GELDİ."
                        );

                        break;
                    }


                    allAirports.AddRange(
                        pageResult.data
                    );


                    Console.WriteLine(
                        $"SAYFA {page}: " +
                        $"{pageResult.data.Length} kayıt"
                    );


                    Console.WriteLine(
                        $"ŞU ANA KADAR TOPLAM: " +
                        $"{allAirports.Count}"
                    );


                    // ========================================
                    // PAGINATION KONTROLÜ
                    // ========================================

                    if (pageResult.pagination != null)
                    {
                        int currentPage =
                            pageResult.pagination.current_page;

                        int lastPage =
                            pageResult.pagination.last_page;


                        Console.WriteLine(
                            $"PAGINATION: " +
                            $"{currentPage} / {lastPage}"
                        );


                        // Son sayfadaysak çık
                        if (currentPage >= lastPage)
                        {
                            break;
                        }
                    }


                    page++;


                    // Sonsuz döngü güvenliği
                    if (page > 50)
                    {
                        Console.WriteLine(
                            "50 sayfa limitine ulaşıldı."
                        );

                        break;
                    }
                }


                Console.WriteLine(
                    "API'DEN GELEN HAM TOPLAM: " +
                    allAirports.Count
                );


                // ============================================
                // AYNI HAVALİMANLARINI TEMİZLE
                // ============================================

                var uniqueAirports =
                    allAirports
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(
                                x.name
                            )
                        )
                        .GroupBy(x =>
                            !string.IsNullOrWhiteSpace(
                                x.iata
                            )
                                ? x.iata.Trim()
                                : x.name.Trim()
                        )
                        .Select(x =>
                            x.First()
                        )
                        .ToList();


                Console.WriteLine(
                    "TEKRARSIZ HAVALİMANI: " +
                    uniqueAirports.Count
                );


                // ============================================
                // DROPDOWN HAZIRLA
                // ============================================

                var airportList =
                    uniqueAirports
                        .OrderBy(x =>
                            x.region
                        )
                        .ThenBy(x =>
                            x.name
                        )
                        .Select(x =>
                            new SelectListItem
                            {
                                Value =
                                    !string.IsNullOrWhiteSpace(
                                        x.iata
                                    )
                                        ? x.iata.Trim()
                                        : x.name.Trim(),

                                Text =
                                    !string.IsNullOrWhiteSpace(
                                        x.iata
                                    )
                                        ? $"{x.region} - {x.name} ({x.iata})"
                                        : $"{x.region} - {x.name}"
                            }
                        )
                        .ToList();


                ViewBag.AirportList =
                    airportList;


                // ============================================
                // KOORDİNATLARI JAVASCRIPT İÇİN HAZIRLA
                // ============================================

                var coordinates =
                    uniqueAirports
                        .Where(x =>
                            !string.IsNullOrWhiteSpace(
                                x.iata
                            )
                        )
                        .GroupBy(x =>
                            x.iata.Trim()
                        )
                        .ToDictionary(
                            x => x.Key,

                            x => new
                            {
                                latitude =
                                    x.First().latitude,

                                longitude =
                                    x.First().longitude
                            }
                        );


                ViewBag.AirportCoordinatesJson =
                    JsonSerializer.Serialize(
                        coordinates
                    );


                Console.WriteLine(
                    "DROPDOWN TOPLAM: " +
                    airportList.Count
                );


                Console.WriteLine(
                    "KOORDİNAT SAYISI: " +
                    coordinates.Count
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "AIRPORT API HATASI:"
                );

                Console.WriteLine(
                    ex.ToString()
                );


                ViewBag.AirportError =
                    ex.Message;


                ViewBag.AirportList =
                    new List<SelectListItem>();


                ViewBag.AirportCoordinatesJson =
                    "{}";
            }


            return View(
                sliderValues
            );
        }


        // ================================================
        // TEK SAYFA AIRPORT VERİSİ ÇEK
        // ================================================

        private async Task<GetAirportsViewModel.Rootobject>
            GetAirportPage(
                HttpClient client,
                string url,
                string apiKey,
                string apiHost)
        {
            using var request =
                new HttpRequestMessage
                {
                    Method =
                        HttpMethod.Get,

                    RequestUri =
                        new Uri(url)
                };


            request.Headers.Add(
                "x-rapidapi-key",
                apiKey
            );


            request.Headers.Add(
                "x-rapidapi-host",
                apiHost
            );


            using var response =
                await client.SendAsync(
                    request
                );


            var json =
                await response.Content
                    .ReadAsStringAsync();


            Console.WriteLine(
                $"AIRPORT URL: {url}"
            );


            Console.WriteLine(
                $"STATUS: " +
                $"{(int)response.StatusCode} " +
                $"{response.StatusCode}"
            );


            if (!response.IsSuccessStatusCode)
            {
                var errorMessage =
                    $"Airport API Hatası: HTTP {(int)response.StatusCode} " +
                    $"{response.StatusCode} - {json}";

                Console.WriteLine(
                    "======================================"
                );

                Console.WriteLine(
                    errorMessage
                );

                Console.WriteLine(
                    "======================================"
                );

                throw new Exception(
                    errorMessage
                );
            }


            var options =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive =
                        true
                };


            return JsonSerializer.Deserialize<
                GetAirportsViewModel.Rootobject
            >(
                json,
                options
            );
        }
    }
}