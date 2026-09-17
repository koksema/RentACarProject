using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.LocationQueries;
using CQRS_RentACar.CQRSPattern.Results.LocationResults;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers
{
    public class GetLocationByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetLocationByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetLocationByIdQueryResult> Handle(GetLocationByIdQuery query)
        {
            var value = await _context.Locations.FirstOrDefaultAsync(x => x.LocationID == query.LocationId);//Veritabanındaki Abouts tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın AboutId değeri benim sana dışarıdan gönderdiğim query.AboutId değerine eşit olsun.
            return new GetLocationByIdQueryResult
            {
                LocationID = value.LocationID,
                Name = value.Name,
                Iata = value.Iata,
                Iaco = value.Iaco,
                Latitude = value.Latitude,
                Longitude = value.Longitude,
                Country = value.Country,
                Elevation = value.Elevation,
                Timezone = value.Timezone
            };

        }
    }
}
