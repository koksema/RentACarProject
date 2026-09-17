using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.BookingQueries;
using CQRS_RentACar.CQRSPattern.Results.BookingResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class GetBookingByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetBookingByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetBookingByIdQueryResult> Handle(GetBookingByIdQuery query)
        {
            var value = await _context.Bookings.FirstOrDefaultAsync(x => x.BookingId == query.BookingId);//Veritabanındaki Bookings tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın BookingId değeri benim sana dışarıdan gönderdiğim query.BookingId değerine eşit olsun.
            return new GetBookingByIdQueryResult
            {
                BookingId = value.BookingId,
                PickUpLocation=value.PickUpLocation,
                DropOffLocation=value.DropOffLocation,
                PickUpDate=value.PickUpDate,
                DropOffDate=value.DropOffDate,
                CarsId=value.CarsId,
                CarModel=value.Car.Model,
                CarBrand=value.Car.Brand,
            };

        }
    }
}
