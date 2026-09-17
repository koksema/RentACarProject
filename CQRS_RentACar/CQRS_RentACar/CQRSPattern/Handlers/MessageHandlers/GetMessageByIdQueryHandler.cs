using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.MessageQueries;
using CQRS_RentACar.CQRSPattern.Results.MessageResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
{
    public class GetMessageByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetMessageByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetMessageByIdQueryResult> Handle(GetMessageByIdQuery query)
        {
            var value = await _context.Messages.FirstOrDefaultAsync(x => x.MessageId == query.MessageId);//Veritabanındaki Messages tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın MessageId değeri benim sana dışarıdan gönderdiğim query.MessageId değerine eşit olsun.
            return new GetMessageByIdQueryResult
            {
                MessageId = value.MessageId,
                Name = value.Name,
                Surname = value.Surname,
                Email = value.Email,
                Phone = value.Phone,
                Subject = value.Subject,
                MessageDeatil = value.MessageDeatil,

            };

        }
    }
}
