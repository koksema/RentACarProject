using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;
using CQRS_RentACar.CQRSPattern.Queries.AboutQueries;
using CQRS_RentACar.CQRSPattern.Results.AboutResults;
using CQRS_RentACar.Entities;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers
{
    public class GetAboutByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetAboutByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetAboutByIdQueryResult> Handle(GetAboutByIdQuery query)
        {
            var value = await _context.Abouts.FirstOrDefaultAsync(x => x.AboutId == query.AboutId);//Veritabanındaki Abouts tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın AboutId değeri benim sana dışarıdan gönderdiğim query.AboutId değerine eşit olsun.
            return new GetAboutByIdQueryResult
            {
                AboutId = value.AboutId,
                Description = value.Description,
                Title = value.Title,

            };

        }
    }
}
