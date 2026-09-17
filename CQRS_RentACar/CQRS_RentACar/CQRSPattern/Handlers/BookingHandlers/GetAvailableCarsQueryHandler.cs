//using CQRS_RentACar.Context;
//using CQRS_RentACar.CQRSPattern.Queries.BookingQueries;
//using CQRS_RentACar.CQRSPattern.Queries.CarsQueries;
//using CQRS_RentACar.CQRSPattern.Results.BookingResults;
//using CQRS_RentACar.CQRSPattern.Results.CarsResults;
//using Microsoft.EntityFrameworkCore;

//namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
//{
//    public class GetAvailableCarsQueryHandler
//    {
//        private readonly DemoContext _context;

//        public GetAvailableCarsQueryHandler(DemoContext context)
//        {
//            _context = context;
//        }

//        public async Task<List<GetAvailableCarsQueryResult>> Handle(GetAvailableCarsQuery query)
//        {
//            // Seçilen tarihlerde dolu olan araç ID'lerini alıyoruz
//            var busyCarIds = await _context.Bookings
//                .Where(b => b.PickUpDate < query.DropOffDate && b.DropOffDate > query.PickUpDate)
//                .Select(b => b.CarsId)
//                .ToListAsync();

//            // Dolu olmayan araçları senin alan isimlerinle çekiyoruz
//            var availableCars = await _context.Cars
//                .Where(c => !busyCarIds.Contains(c.CarsId))
//                .Select(c => new GetAvailableCarsQueryResult
//                {
//                    CarsId = c.CarsId,
//                    CarsImg = c.CarsImg,
//                    Brand = c.Brand,
//                    Model = c.Model,
//                    Review = c.Review,
//                    Price = c.Price,
//                    Seat = c.Seat,
//                    Transmission = c.Transmission,
//                    Fuel = c.Fuel,
//                    Year = c.Year,
//                    Km = c.Km
//                })
//                .ToListAsync();

//            return availableCars;
//        }
//    }
//}
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.BookingQueries;
using CQRS_RentACar.CQRSPattern.Results.BookingResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class GetAvailableCarsQueryHandler
    {
        private readonly DemoContext _context;

        public GetAvailableCarsQueryHandler(
            DemoContext context)
        {
            _context = context;
        }

        public async Task<List<GetAvailableCarsQueryResult>> Handle(
            GetAvailableCarsQuery query)
        {
            // Seçilen tarih aralığında dolu olan araç ID'leri
            var busyCarIds =
                await _context.Bookings
                    .Where(b =>
                        b.PickUpDate < query.DropOffDate &&
                        b.DropOffDate > query.PickUpDate
                    )
                    .Select(b => b.CarsId)
                    .Distinct()
                    .ToListAsync();


            // Dolu olmayan araçları getir
            var availableCars =
                await _context.Cars
                    .Where(c =>
                        !busyCarIds.Contains(c.CarsId)
                    )
                    .OrderBy(c => c.Price)
                    .Select(c =>
                        new GetAvailableCarsQueryResult
                        {
                            CarsId = c.CarsId,
                            CarsImg = c.CarsImg,
                            Brand = c.Brand,
                            Model = c.Model,
                            Review = c.Review,
                            Price = c.Price,
                            Seat = c.Seat,
                            Transmission = c.Transmission,
                            Fuel = c.Fuel,
                            Year = c.Year,
                            Km = c.Km
                        }
                    )
                    .ToListAsync();


            return availableCars;
        }
    }
}