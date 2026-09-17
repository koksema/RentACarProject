using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;
using CQRS_RentACar.CQRSPattern.Handlers.FuelHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers;
using CQRS_RentACar.CQRSPattern.Queries.FuelQueries;
using CQRS_RentACar.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CQRS_RentACar.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DemoContext _context;
        private readonly IConfiguration _configuration;
        private readonly GetFuelPricesQueryHandler _handler;
        private readonly CreateMessageCommandHandler
            _createMessageHandler;

        public DashboardController(
            DemoContext context,
            IConfiguration configuration,
            GetFuelPricesQueryHandler handler,
            CreateMessageCommandHandler createMessageHandler)
        {
            _context = context;
            _configuration = configuration;
            _handler = handler;
            _createMessageHandler =
                createMessageHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // =====================================================
            // GENEL İSTATİSTİKLER
            // =====================================================

            try
            {
                ViewBag.TotalCarCount =
                    await _context.Cars.CountAsync();

                ViewBag.TotalBrandCount =
                    await _context.Cars
                        .Where(x => x.Brand != null)
                        .Select(x => x.Brand)
                        .Distinct()
                        .CountAsync();

                ViewBag.TotalMessageCount =
                    await _context.Messages.CountAsync();

                ViewBag.TotalBookingCount =
                    await _context.Bookings.CountAsync();

                ViewBag.AverageCarPrice =
                    await _context.Cars.AnyAsync()
                        ? await _context.Cars.AverageAsync(x => x.Price)
                        : 0;

                ViewBag.StatisticError = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                ViewBag.TotalCarCount = 0;
                ViewBag.TotalBrandCount = 0;
                ViewBag.TotalMessageCount = 0;
                ViewBag.TotalBookingCount = 0;
                ViewBag.AverageCarPrice = 0;

                ViewBag.StatisticError =
                    "Dashboard istatistikleri alınamadı.";
            }


            // =====================================================
            // YAKIT FİYATLARI
            // =====================================================

            try
            {
                var result =
                    await _handler.Handle(
                        new GetFuelPricesQuery()
                    );

                var gasoline =
                    result.FuelPrices
                        .FirstOrDefault(x =>
                            x.FuelType == "Gasoline");

                var diesel =
                    result.FuelPrices
                        .FirstOrDefault(x =>
                            x.FuelType == "Diesel");

                ViewBag.GasolinePrice =
                    gasoline != null
                        ? gasoline.Price.ToString("N2") + " ₺"
                        : "Veri Bulunamadı";

                ViewBag.DieselPrice =
                    diesel != null
                        ? diesel.Price.ToString("N2") + " ₺"
                        : "Veri Bulunamadı";

                ViewBag.FuelProvince =
                    result.Province ?? "İstanbul";

                ViewBag.FuelDate =
                    result.Date ?? "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    ex.ToString()
                );

                ViewBag.GasolinePrice =
                    "Servis Dışı";

                ViewBag.DieselPrice =
                    "Servis Dışı";

                ViewBag.FuelProvince = "";
                ViewBag.FuelDate = "";
            }


            // =====================================================
            // REZERVASYON DURUMLARI
            // =====================================================

            try
            {
                var today = DateTime.Now;

                ViewBag.UpcomingBookingCount =
                    await _context.Bookings
                        .CountAsync(x =>
                            x.PickUpDate > today);

                ViewBag.ActiveBookingCount =
                    await _context.Bookings
                        .CountAsync(x =>
                            x.PickUpDate <= today &&
                            x.DropOffDate >= today);

                ViewBag.CompletedBookingCount =
                    await _context.Bookings
                        .CountAsync(x =>
                            x.DropOffDate < today);
            }
            catch
            {
                ViewBag.UpcomingBookingCount = 0;
                ViewBag.ActiveBookingCount = 0;
                ViewBag.CompletedBookingCount = 0;
            }


            // =====================================================
            // SON 6 AY REZERVASYON GRAFİĞİ
            // =====================================================

            try
            {
                var monthLabels =
                    new List<string>();

                var monthValues =
                    new List<int>();

                var trCulture =
                    new System.Globalization.CultureInfo(
                        "tr-TR"
                    );

                for (int i = 5; i >= 0; i--)
                {
                    var month =
                        DateTime.Today.AddMonths(-i);

                    var startDate =
                        new DateTime(
                            month.Year,
                            month.Month,
                            1
                        );

                    var endDate =
                        startDate.AddMonths(1);

                    var count =
                        await _context.Bookings
                            .CountAsync(x =>
                                x.PickUpDate >= startDate &&
                                x.PickUpDate < endDate
                            );

                    monthLabels.Add(
                        startDate.ToString(
                            "MMM yyyy",
                            trCulture
                        )
                    );

                    monthValues.Add(count);
                }

                ViewBag.MonthLabelsJson =
                    JsonSerializer.Serialize(
                        monthLabels
                    );

                ViewBag.MonthValuesJson =
                    JsonSerializer.Serialize(
                        monthValues
                    );
            }
            catch
            {
                ViewBag.MonthLabelsJson = "[]";
                ViewBag.MonthValuesJson = "[]";
            }


            // =====================================================
            // SON EKLENEN 5 ARAÇ
            // =====================================================

            try
            {
                ViewBag.RecentCars =
                    await _context.Cars
                        .OrderByDescending(x =>
                            x.CarsId)
                        .Take(5)
                        .ToListAsync();
            }
            catch
            {
                ViewBag.RecentCars =
                    new List<CQRS_RentACar.Entities.Cars>();
            }


            // =====================================================
            // SON 5 REZERVASYON
            // =====================================================

            try
            {
                ViewBag.RecentBookings =
                    await _context.Bookings
                        .Include(x => x.Car)
                        .OrderByDescending(x =>
                            x.BookingId)
                        .Take(5)
                        .ToListAsync();
            }
            catch
            {
                ViewBag.RecentBookings =
                    new List<CQRS_RentACar.Entities.Booking>();
            }


            return View();
        }

        [HttpGet]
        public IActionResult CarRecommendation()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetCarRecommendation(string userPrompt)
        {
            if (string.IsNullOrWhiteSpace(userPrompt))
            {
                return Json(new
                {
                    success = false,
                    message = "Lütfen geçerli bir istek girin."
                });
            }

            try
            {
                var cars = await _context.Cars
                    .Select(c => new CarItemDto
                    {
                        Id = c.CarsId,
                        Brand = c.Brand,
                        Model = c.Model,
                        CarsImg = c.CarsImg,
                        Seat = c.Seat.ToString(),
                        Transmission = c.Transmission,
                        Fuel = c.Fuel,
                        Price = c.Price
                    })
                    .ToListAsync();

                if (cars.Count == 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Filoda önerilebilecek araç bulunamadı."
                    });
                }

                var apiKey =
                    _configuration["Gemini:CarRecommendationApiKey"];

                if (string.IsNullOrWhiteSpace(apiKey))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Gemini API anahtarı bulunamadı."
                    });
                }

                var bot =
                    new ChatbotModel(apiKey);

                var recommendation =
                    await bot.AskGeminiAsync(
                        userPrompt,
                        cars
                    );

                if (string.IsNullOrWhiteSpace(recommendation))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Araç önerisi oluşturulamadı."
                    });
                }

                if (recommendation.StartsWith(
                        "Gemini API Hatası",
                        StringComparison.OrdinalIgnoreCase) ||
                    recommendation.StartsWith(
                        "Gemini bağlantı hatası",
                        StringComparison.OrdinalIgnoreCase) ||
                    recommendation.StartsWith(
                        "Sistem Hatası",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new
                    {
                        success = false,
                        message = recommendation
                    });
                }


                // =========================================
                // GEMINI'NİN ÖNERDİĞİ ARAÇ ID'LERİNİ BUL
                // =========================================

                var selectedIds =
                    System.Text.RegularExpressions.Regex
                        .Matches(
                            recommendation,
                            @"ID\s*:\s*(\d+)",
                            System.Text.RegularExpressions.RegexOptions.IgnoreCase
                        )
                        .Select(x =>
                            int.Parse(
                                x.Groups[1].Value
                            )
                        )
                        .Distinct()
                        .ToList();


                var selectedCars =
                    cars
                        .Where(x =>
                            selectedIds.Contains(x.Id)
                        )
                        .Select(x => new
                        {
                            id = x.Id,
                            brand = x.Brand,
                            model = x.Model,
                            carsImg = x.CarsImg,
                            seat = x.Seat,
                            transmission = x.Transmission,
                            fuel = x.Fuel,
                            price = x.Price
                        })
                        .ToList();


                return Json(new
                {
                    success = true,
                    recommendation,
                    selectedCars
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "ARAÇ ÖNERİ HATASI: " +
                    ex
                );

                return Json(new
                {
                    success = false,
                    message =
                        "Araç önerisi alınırken hata oluştu: " +
                        ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult>
            SendMessage(
                CreateMessageCommand command)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message =
                        "Lütfen tüm alanları doğru doldurun."
                });
            }

            await _createMessageHandler
                .Handle(command);

            return Json(new
            {
                success = true,
                message =
                    "Mesajınız başarıyla iletildi."
            });
        }


        [HttpGet]
        public async Task<IActionResult>
            MessageList()
        {
            var messages =
                await _context.Messages
                    .ToListAsync();

            return View(messages);
        }


        [HttpPost]
        public async Task<IActionResult>
            GenerateAiReply(int id)
        {
            var userMessage =
                await _context.Messages
                    .FindAsync(id);

            if (userMessage == null)
            {
                return Json(new
                {
                    success = false,
                    message =
                        "Mesaj bulunamadı."
                });
            }

            var apiKey =
                _configuration[
                    "Gemini:MessageReplyApiKey"
                ];

            string aiReply = "";

            try
            {
                using var client =
                    new HttpClient();

                var requestUrl =
                    "https://generativelanguage.googleapis.com/" +
                    "v1beta/models/gemini-1.5-flash:" +
                    $"generateContent?key={apiKey}";

                var prompt =
                    $"Müşteri Adı Soyadı: " +
                    $"{userMessage.Name} " +
                    $"{userMessage.Surname}\n" +

                    $"Konu: " +
                    $"{userMessage.Subject}\n" +

                    $"Müşteri Mesajı: " +
                    $"{userMessage.MessageDeatil}\n\n" +

                    "Sen bir araç kiralama " +
                    "şirketinin müşteri temsilcisisin. " +
                    "Kurumsal, kibar, profesyonel ve " +
                    "çözüm odaklı bir cevap hazırla.";

                var requestBody =
                    new
                    {
                        contents =
                            new[]
                            {
                                new
                                {
                                    parts =
                                        new[]
                                        {
                                            new
                                            {
                                                text = prompt
                                            }
                                        }
                                }
                            }
                    };

                var jsonContent =
                    new StringContent(
                        JsonSerializer.Serialize(
                            requestBody
                        ),
                        Encoding.UTF8,
                        "application/json"
                    );

                var response =
                    await client.PostAsync(
                        requestUrl,
                        jsonContent
                    );

                if (response.IsSuccessStatusCode)
                {
                    var responseString =
                        await response.Content
                            .ReadAsStringAsync();

                    using var doc =
                        JsonDocument.Parse(
                            responseString
                        );

                    aiReply =
                        doc.RootElement
                            .GetProperty(
                                "candidates"
                            )[0]
                            .GetProperty(
                                "content"
                            )
                            .GetProperty(
                                "parts"
                            )[0]
                            .GetProperty(
                                "text"
                            )
                            .GetString();
                }
                else
                {
                    aiReply =
                        $"Sayın {userMessage.Name} " +
                        $"{userMessage.Surname}, " +
                        "mesajınız alınmıştır. " +
                        "En kısa sürede dönüş yapacağız.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    ex.ToString()
                );

                aiReply =
                    $"Sayın {userMessage.Name} " +
                    $"{userMessage.Surname}, " +
                    "talebiniz incelenmektedir.";
            }

            return Json(new
            {
                success = true,
                aiReply
            });
        }
    }
}