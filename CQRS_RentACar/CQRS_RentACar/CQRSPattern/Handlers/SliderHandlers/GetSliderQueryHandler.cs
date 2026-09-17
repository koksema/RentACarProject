using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.SliderResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers
{
    public class GetSliderQueryHandler
    {
        private readonly DemoContext _context;

        public GetSliderQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetSliderQueryResult>> Handle()
        {
            var value = await _context.Sliders.ToListAsync();
            return value.Select(x => new GetSliderQueryResult
            {
                SliderId = x.SliderId,
                Title = x.Title,
                ImgUrl = x.ImgUrl,
            }).ToList();

        }
    }
}
