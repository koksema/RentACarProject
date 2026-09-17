using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.SliderQueries;
using CQRS_RentACar.CQRSPattern.Results.SliderResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers
{
    public class GetSliderByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetSliderByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetSliderByIdQueryResult> Handle(GetSliderByIdQuery query)
        {
            var value = await _context.Sliders.FirstOrDefaultAsync(x => x.SliderId == query.SliderId);//Veritabanındaki Sliders tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın SliderId değeri benim sana dışarıdan gönderdiğim query.SliderId değerine eşit olsun.
            return new GetSliderByIdQueryResult
            {
                SliderId = value.SliderId,
                Title = value.Title,
                ImgUrl = value.ImgUrl,

            };

        }
    }
}
