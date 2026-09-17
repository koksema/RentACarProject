//using CQRS_RentACar.Context;
//using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;
//using CQRS_RentACar.Entities;

//namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
//{
//    public class CreateMessageCommandHandler
//    {
//        private readonly DemoContext _context;

//        public CreateMessageCommandHandler(DemoContext context)
//        {
//            _context = context;
//        }
//        public async Task Handle(CreateMessageCommand command)
//        {
//            _context.Messages.Add(new Message
//            {
//                Name = command.Name,
//                Surname = command.Surname,
//                Email = command.Email,
//                Phone = command.Phone,
//                Subject = command.Subject,
//                MessageDeatil = command.MessageDeatil,
//            });
//            await _context.SaveChangesAsync();
//        }
//    }
//}
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
{
    public class CreateMessageCommandHandler
    {
        private readonly DemoContext _context;

        public CreateMessageCommandHandler(
            DemoContext context)
        {
            _context = context;
        }


        public async Task<Message> Handle(
            CreateMessageCommand command)
        {
            var message = new Message
            {
                Name = command.Name,

                Surname = command.Surname,

                Email = command.Email,

                Phone = command.Phone,

                Subject = command.Subject,

                MessageDeatil =
                    command.MessageDeatil,

                MessageDate =
                    DateTime.Now,

                IsRead = false,

                AiReply = null
            };


            _context.Messages.Add(
                message
            );


            await _context.SaveChangesAsync();


            return message;
        }
    }
}