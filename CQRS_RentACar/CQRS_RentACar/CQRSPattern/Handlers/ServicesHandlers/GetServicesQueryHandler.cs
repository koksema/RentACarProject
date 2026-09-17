using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.ServicesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers
{
    public class GetServicesQueryHandler
    {
        private readonly DemoContext _context;

        public GetServicesQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetServicesQueryResult>> Handle()
        {
            var value = await _context.Services.ToListAsync();
            return value.Select(x => new GetServicesQueryResult
            {
                ServicesId = x.ServicesId,
                Title = x.Title,
                Description = x.Description,
                IconUrl = x.IconUrl,

            }).ToList();

        }
    }
}
