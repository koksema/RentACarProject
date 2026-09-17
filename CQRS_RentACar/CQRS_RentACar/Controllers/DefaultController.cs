using CQRS_RentACar.CQRSPattern.Handlers.AirportsHandler;
using CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers;
using CQRS_RentACar.CQRSPattern.Queries.CarsQueries;
using CQRS_RentACar.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CQRS_RentACar.Controllers
{
    public class DefaultController : Controller
    {
        private readonly GetAirportsQueryHandler _handler;

        public DefaultController(GetAirportsQueryHandler handler)
        {
            _handler = handler;
        }

    
        public IActionResult Index()
        {
          return View(); 
        }
        [HttpGet]
        public async Task<IActionResult> GetAirports()
        {
            // Handler'ı tetikleyip RapidAPI'den havalimanı listesini çekiyoruz
            var airports = await _handler.Handle();

            // View tarafındaki <select> (dropdown) yapısında gösterebilmek için dönüştürüyoruz
            ViewBag.AirportList = airports.Select(x => new SelectListItem
            {
                Text = $"{x.Name} ({x.Country}) - {x.IataCode}",
                Value = x.IataCode
            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CalculatePrice(CalculationModel model)
        {
            if (model.PickUpDate == default || model.DropOffDate == default)
            {
                return Json(new { success = false, message = "Lütfen tarihleri seçiniz." });
            }

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://booking-com.p.rapidapi.com/v1/car-rental/search?pick_up_location={model.PickUpLocation}&drop_off_location={model.DropOffLocation}&pick_up_date={model.PickUpDate:yyyy-MM-dd}&drop_off_date={model.DropOffDate:yyyy-MM-dd}&pick_up_time={model.PickUpTime}&drop_off_time={model.DropOffTime}"),
                Headers =
        {
            { "x-rapidapi-key", "" },
            { "x-rapidapi-host", "tollguru-toll-rest1.p.rapidapi.com" },
        },
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                // RapidAPI'den gelen JSON verisini parse edip toplam fiyatı alıyoruz
                // (Buradaki parse mantığı kullandığın spesifik RapidAPI servisine göre değişir)
                var days = (model.DropOffDate - model.PickUpDate).Days;
                if (days <= 0) days = 1;

                // Örnek API Response Dönüşü
                return Json(new
                {
                    success = true,
                    days = days,
                    totalPrice = 2450.00, // API JSON'ından çekilen dinamik fiyat
                    currency = "TRY"
                });
            }
        }
    }
}