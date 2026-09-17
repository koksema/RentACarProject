using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.AboutQueries;
using CQRS_RentACar.CQRSPattern.Queries.CarsQueries;
using CQRS_RentACar.CQRSPattern.Results.AboutResults;
using CQRS_RentACar.CQRSPattern.Results.CarsResults;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers
{
    public class GetCarsByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetCarsByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetCarsByIdQueryResult> Handle(GetCarsByIdQuery query)
        {
            var value = await _context.Cars.FirstOrDefaultAsync(x => x.CarsId == query.CarsId);//Veritabanındaki Abouts tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın AboutId değeri benim sana dışarıdan gönderdiğim query.AboutId değerine eşit olsun.
            return new GetCarsByIdQueryResult
            {
                CarsId = value.CarsId,
                CarsImg = value.CarsImg,
                Brand = value.Brand,
                Model = value.Model,
                Review = value.Review,
                Price = value.Price,
                Seat = value.Seat,
                Transmission = value.Transmission,
                Fuel = value.Fuel,
                Year = value.Year,
                Km = value.Km,

            };

        }
    }
}
//CQRS standartlarında "gelen talebi işleyen" metotlara Handle denir