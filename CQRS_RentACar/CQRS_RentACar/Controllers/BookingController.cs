using System.Net;
using System.Threading.Tasks;
using CQRS_RentACar.CQRSPattern.Commands.BookingCommands;
using CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers;
using CQRS_RentACar.CQRSPattern.Handlers.FuelHandlers;
using CQRS_RentACar.CQRSPattern.Queries.BookingQueries;
using CQRS_RentACar.CQRSPattern.Queries.CarsQueries;
using CQRS_RentACar.CQRSPattern.Queries.FuelQueries;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_RentACar.Controllers
{
    public class BookingController : Controller
    {
        private readonly GetBookingQueryHandler _getBookingQueryHandler;
        private readonly CreateBookingCommandHandler _createBookingCommandHandler;
        private readonly GetBookingByIdQueryHandler _getBookingByIdQueryHandler;
        private readonly RemoveBookingCommandHandler _removeBookingCommandHandler;
        private readonly UpdateBookingCommandHandler _updateBookingCommandHandler;
        private readonly GetAvailableCarsQueryHandler _getAvailableCarsQueryHandler;
        private readonly GetFuelPricesQueryHandler _getFuelPricesQueryHandler;

        public BookingController(GetBookingQueryHandler getBookingQueryHandler, CreateBookingCommandHandler createBookingCommandHandler, GetBookingByIdQueryHandler getBookingByIdQueryHandler, RemoveBookingCommandHandler removeBookingCommandHandler, UpdateBookingCommandHandler updateBookingCommandHandler, GetAvailableCarsQueryHandler getAvailableCarsQueryHandler, GetFuelPricesQueryHandler getFuelPricesQueryHandler)
        {
            _getBookingQueryHandler = getBookingQueryHandler;
            _createBookingCommandHandler = createBookingCommandHandler;
            _getBookingByIdQueryHandler = getBookingByIdQueryHandler;
            _removeBookingCommandHandler = removeBookingCommandHandler;
            _updateBookingCommandHandler = updateBookingCommandHandler;
            _getAvailableCarsQueryHandler = getAvailableCarsQueryHandler;
            _getFuelPricesQueryHandler = getFuelPricesQueryHandler;
        }

        public async Task<IActionResult> AllBookings()
        {
            var value = await _getBookingQueryHandler.Handle();
            return View(value);
        }
        [HttpGet]
        public IActionResult CreateBooking(
     int carId,
     string pickUpLocation,
     string dropOffLocation,
     DateTime pickUpDate,
     DateTime dropOffDate)
        {
            ViewBag.CarId = carId;
            ViewBag.PickUpLocation = pickUpLocation;
            ViewBag.DropOffLocation = dropOffLocation;
            ViewBag.PickUpDate = pickUpDate;
            ViewBag.DropOffDate = dropOffDate;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking(
      CreateBookingCommand command)
        {
            try
            {
                if (command.CarId <= 0)
                {
                    TempData["BookingError"] =
                        "Araç bilgisi alınamadı.";

                    return View(command);
                }

                if (command.DropOffDate <= command.PickUpDate)
                {
                    TempData["BookingError"] =
                        "Teslim tarihi alış tarihinden sonra olmalıdır.";

                    return View(command);
                }

                await _createBookingCommandHandler.Handle(command);

                TempData["BookingSuccess"] =
                    "Rezervasyonunuz başarıyla oluşturuldu.";

                return RedirectToAction(
                    "Index",
                    "Default"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "CREATE BOOKING HATASI:"
                );

                Console.WriteLine(
                    ex.ToString()
                );

                TempData["BookingError"] =
                    "Rezervasyon oluşturulurken bir hata oluştu.";

                return View(command);
            }
        }
        public async Task<IActionResult> DeleteBooking(RemoveBookingCommand command)
        {
            await _removeBookingCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBooking(int id)
        {
            var value = await _getBookingByIdQueryHandler.Handle(new GetBookingByIdQuery(id));
            return View(value);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBooking(UpdateBookingCommand command)
        {
            await _updateBookingCommandHandler.Handle(command);
            return RedirectToAction("Index");
        }
        private string DecodeHtmlEntities(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return WebUtility.HtmlDecode(text);
        }
        [HttpPost]
        public async Task<IActionResult> AvailableCars(
    DateTime pickUpDate,
    DateTime dropOffDate,
    string pickUpTime,
    string dropOffTime,
    string pickUpLocation,
    string dropOffLocation)
        {
            try
            {
                // Lokasyon kontrolü
                if (string.IsNullOrWhiteSpace(pickUpLocation))
                {
                    TempData["BookingError"] =
                        "Lütfen alış lokasyonunu seçin.";

                    return RedirectToAction(
                        "Index",
                        "Default"
                    );
                }

                if (string.IsNullOrWhiteSpace(dropOffLocation))
                {
                    dropOffLocation =
                        pickUpLocation;
                }


                // HTML entity temizleme
                pickUpLocation =
                    DecodeHtmlEntities(
                        pickUpLocation
                    );

                dropOffLocation =
                    DecodeHtmlEntities(
                        dropOffLocation
                    );


                // Saatleri güvenli parse et
                TimeSpan pickUpTimeSpan =
                    TimeSpan.Zero;

                TimeSpan dropOffTimeSpan =
                    TimeSpan.Zero;


                if (!string.IsNullOrWhiteSpace(
                    pickUpTime))
                {
                    DateTime parsedPickUpTime;

                    if (DateTime.TryParse(
                        pickUpTime,
                        out parsedPickUpTime))
                    {
                        pickUpTimeSpan =
                            parsedPickUpTime.TimeOfDay;
                    }
                }


                if (!string.IsNullOrWhiteSpace(
                    dropOffTime))
                {
                    DateTime parsedDropOffTime;

                    if (DateTime.TryParse(
                        dropOffTime,
                        out parsedDropOffTime))
                    {
                        dropOffTimeSpan =
                            parsedDropOffTime.TimeOfDay;
                    }
                }


                var pickUpDateTime =
                    pickUpDate.Date
                        .Add(pickUpTimeSpan);

                var dropOffDateTime =
                    dropOffDate.Date
                        .Add(dropOffTimeSpan);


                // Tarih kontrolü
                if (dropOffDateTime <=
                    pickUpDateTime)
                {
                    TempData["BookingError"] =
                        "Teslim tarihi ve saati, " +
                        "alış tarihinden sonra olmalıdır.";

                    return RedirectToAction(
                        "Index",
                        "Default"
                    );
                }


                // CQRS Query
                var query =
                    new GetAvailableCarsQuery(
                        pickUpDateTime,
                        dropOffDateTime,
                        pickUpLocation
                    );


                var values =
                    await _getAvailableCarsQueryHandler
                        .Handle(query);


                // ViewBag bilgileri
                ViewBag.PickUpDate =
                    pickUpDateTime;

                ViewBag.DropOffDate =
                    dropOffDateTime;

                ViewBag.PickUpLocation =
                    pickUpLocation;

                ViewBag.DropOffLocation =
                    dropOffLocation;


                return View(values);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "AVAILABLE CARS HATASI: " +
                    ex.ToString()
                );

                TempData["BookingError"] =
                    "Müsait araçlar alınırken " +
                    "bir hata oluştu.";

                return RedirectToAction(
                    "Index",
                    "Default"
                );
            }
        }
        [HttpPost]
        public async Task<IActionResult> CalculateTripEstimate(
     double distanceKm)
        {
            try
            {
                if (distanceKm <= 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Geçerli bir mesafe hesaplanamadı."
                    });
                }

                const decimal averageConsumption = 7.0m;

                decimal distance =
                    Convert.ToDecimal(distanceKm);

                decimal requiredLiters =
                    (distance / 100m) *
                    averageConsumption;


                // ===============================
                // YAKIT API
                // ===============================

                var fuelResult =
                    await _getFuelPricesQueryHandler.Handle(
                        new GetFuelPricesQuery()
                    );


                if (fuelResult == null)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Yakıt API sonucu null geldi."
                    });
                }


                if (fuelResult.FuelPrices == null)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Yakıt fiyat listesi null geldi."
                    });
                }


                var gasoline =
                    fuelResult.FuelPrices
                        .FirstOrDefault(x =>
                            x.FuelType == "Gasoline"
                        );


                var diesel =
                    fuelResult.FuelPrices
                        .FirstOrDefault(x =>
                            x.FuelType == "Diesel"
                        );


                if (gasoline == null)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Benzin fiyatı API'den alınamadı."
                    });
                }


                if (diesel == null)
                {
                    return Json(new
                    {
                        success = false,
                        message =
                            "Motorin fiyatı API'den alınamadı."
                    });
                }


                decimal gasolineCost =
                    requiredLiters *
                    gasoline.Price;


                decimal dieselCost =
                    requiredLiters *
                    diesel.Price;


                return Json(new
                {
                    success = true,

                    distance =
                        Math.Round(
                            distanceKm,
                            0
                        ),

                    liters =
                        Math.Round(
                            requiredLiters,
                            2
                        ),

                    gasolinePrice =
                        gasoline.Price,

                    dieselPrice =
                        diesel.Price,

                    gasolineCost =
                        Math.Round(
                            gasolineCost,
                            2
                        ),

                    dieselCost =
                        Math.Round(
                            dieselCost,
                            2
                        ),

                    province =
                        fuelResult.Province,

                    fuelDate =
                        fuelResult.Date
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "========== TRIP ESTIMATE HATASI =========="
                );

                Console.WriteLine(
                    ex.ToString()
                );

                Console.WriteLine(
                    "=========================================="
                );


                return Json(new
                {
                    success = false,

                    // ŞİMDİLİK GERÇEK HATAYI GÖSTER
                    message =
                        "HATA: " +
                        ex.Message
                });
            }
        
    }
       
    }
}
