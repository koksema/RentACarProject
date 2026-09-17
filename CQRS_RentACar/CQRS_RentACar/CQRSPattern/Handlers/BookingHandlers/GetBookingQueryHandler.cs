using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.BookingResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class GetBookingQueryHandler
    {
        private readonly DemoContext _context;

        public GetBookingQueryHandler(
            DemoContext context)
        {
            _context = context;
        }


        public async Task<List<GetBookingQueryResult>>
            Handle()
        {
            var values =
                await _context.Bookings

                    // Araç bilgilerini de getir
                    .Include(x => x.Car)

                    // En yeni rezervasyon üstte
                    .OrderByDescending(
                        x => x.PickUpDate
                    )

                    .Select(x =>
                        new GetBookingQueryResult
                        {
                            BookingId =
                                x.BookingId,

                            PickUpLocation =
                                x.PickUpLocation,

                            DropOffLocation =
                                x.DropOffLocation,

                            PickUpDate =
                                x.PickUpDate,

                            DropOffDate =
                                x.DropOffDate,

                            CarsId =
                                x.CarsId,

                            CarModel =
                                x.Car != null
                                    ? x.Car.Model
                                    : "Araç Bulunamadı",

                            CarBrand =
                                x.Car != null
                                    ? x.Car.Brand
                                    : ""
                        }
                    )
                    .ToListAsync();


            return values;
        }
    }
}