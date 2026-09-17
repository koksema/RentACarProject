using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.FeaturesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers
{
    public class GetFeaturesQueryHandler
    {
        private readonly DemoContext _context;

        public GetFeaturesQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetFeaturesQueryResult>> Handle()
        {
            var value = await _context.Features.ToListAsync();
            return value.Select(x => new GetFeaturesQueryResult
            {
                FeaturesId = x.FeaturesId,
                Title = x.Title,
                Description = x.Description,
                IconUrl = x.IconUrl,
                IconTitle = x.IconTitle,
                IconDescription = x.IconDescription,
                ImgUrl = x.ImgUrl,

            }).ToList();

        }
    }
}
