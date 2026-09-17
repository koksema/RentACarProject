using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.AboutQueries;
using CQRS_RentACar.CQRSPattern.Results.AboutResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers
{
    public class GetAboutQueryHandler
    {
        private readonly DemoContext _context;

        public GetAboutQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetAboutQueryResult>> Handle()
        {
            var value = await _context.Abouts.ToListAsync();
            return value.Select(x => new GetAboutQueryResult
            {
                AboutId = x.AboutId,
                Title = x.Title,
                Description = x.Description,

            }).ToList();

        }
    }
}
