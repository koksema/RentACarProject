using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.FeaturesQueries;
using CQRS_RentACar.CQRSPattern.Results.FeaturesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers
{
    public class GetFeaturesByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetFeaturesByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetFeaturesByIdQueryResult> Handle(GetFeaturesByIdQuery query)
        {
            var value = await _context.Features.FirstOrDefaultAsync(x => x.FeaturesId == query.FeaturesId);//Veritabanındaki Featuress tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın FeaturesId değeri benim sana dışarıdan gönderdiğim query.FeaturesId değerine eşit olsun.
            return new GetFeaturesByIdQueryResult
            {
                FeaturesId = value.FeaturesId,
                Description = value.Description,
                Title = value.Title,
                IconUrl = value.IconUrl,
                IconTitle= value.IconTitle,
                IconDescription= value.IconDescription,
                ImgUrl = value.ImgUrl,

            };

        }
    }
}
