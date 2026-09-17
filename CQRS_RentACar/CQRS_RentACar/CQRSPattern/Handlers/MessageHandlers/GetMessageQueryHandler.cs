//using CQRS_RentACar.Context;
//using CQRS_RentACar.CQRSPattern.Results.MessageResults;
//using Microsoft.EntityFrameworkCore;

//namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
//{
//    public class GetMessageQueryHandler
//    {
//        private readonly DemoContext _context;

//        public GetMessageQueryHandler(DemoContext context)
//        {
//            _context = context;
//        }
//        public async Task<List<GetMessageQueryResult>> Handle()
//        {
//            var value = await _context.Messages.ToListAsync();
//            return value.Select(x => new GetMessageQueryResult
//            {
//                MessageId = x.MessageId,
//                Name = x.Name,
//                Surname = x.Surname,
//                Email = x.Email,
//                Phone = x.Phone,
//                Subject = x.Subject,
//                MessageDeatil = x.MessageDeatil,

//            }).ToList();

//        }
//    }
//}
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.MessageResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
{
    public class GetMessageQueryHandler
    {
        private readonly DemoContext _context;

        public GetMessageQueryHandler(
            DemoContext context)
        {
            _context = context;
        }


        public async Task<List<GetMessageQueryResult>>
            Handle()
        {
            var values =
                await _context.Messages
                    .OrderByDescending(
                        x => x.MessageDate
                    )
                    .Select(x =>
                        new GetMessageQueryResult
                        {
                            MessageId =
                                x.MessageId,

                            Name =
                                x.Name,

                            Surname =
                                x.Surname,

                            Email =
                                x.Email,

                            Phone =
                                x.Phone,

                            Subject =
                                x.Subject,

                            MessageDeatil =
                                x.MessageDeatil,

                            AiReply =
                                x.AiReply,

                            MessageDate =
                                x.MessageDate,

                            IsRead =
                                x.IsRead
                        }
                    )
                    .ToListAsync();


            return values;
        }
    }
}