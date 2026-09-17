using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.AboutResults;
using CQRS_RentACar.CQRSPattern.Results.CarsResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers
{
    public class GetCarsQueryHandler
    {
        private readonly DemoContext _context;

        public GetCarsQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetCarsQueryResult>> Handle()
        {
            var value = await _context.Cars.ToListAsync();
            return value.Select(x => new GetCarsQueryResult
            {
                CarsId = x.CarsId,
                CarsImg = x.CarsImg,
                Brand = x.Brand,
                Model = x.Model,
                Review = x.Review,
                Price = x.Price,
                Seat = x.Seat,
                Transmission = x.Transmission,
                Fuel = x.Fuel,
                Year = x.Year,
                Km = x.Km,

            }).ToList();

        }
    }
}
