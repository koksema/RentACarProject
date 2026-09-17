using System.Net.Http.Json;
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.FeaturesResults;
using CQRS_RentACar.CQRSPattern.Results.LocationResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers
{
    public class GetLocationQueryHandler
    {
        private readonly DemoContext _context;

        public GetLocationQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetLocationQueryResult>> Handle()
        {
            var value = await _context.Locations.ToListAsync();
            return value.Select(x => new GetLocationQueryResult
            {
                LocationID = x.LocationID,
                Name = x.Name,
                Iata = x.Iata,
                Iaco = x.Iaco,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Country = x.Country,
                Elevation = x.Elevation,
                Timezone = x.Timezone

            }).ToList();

        }
    }
}