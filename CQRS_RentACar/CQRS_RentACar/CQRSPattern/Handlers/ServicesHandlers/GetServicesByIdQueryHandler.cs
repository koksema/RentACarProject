using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.ServicesQueries;
using CQRS_RentACar.CQRSPattern.Results.ServicesResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers
{
    public class GetServicesByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetServicesByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetServicesByIdQueryResult> Handle(GetServicesByIdQuery query)
        {
            var value = await _context.Services.FirstOrDefaultAsync(x => x.ServicesId == query.ServicesId);//Veritabanındaki Servicess tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın ServicesId değeri benim sana dışarıdan gönderdiğim query.ServicesId değerine eşit olsun.
            return new GetServicesByIdQueryResult
            {
                ServicesId = value.ServicesId,
                Description = value.Description,
                Title = value.Title,
                IconUrl = value.IconUrl,

            };

        }
    }
}
