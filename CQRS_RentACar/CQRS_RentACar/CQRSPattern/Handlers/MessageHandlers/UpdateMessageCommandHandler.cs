using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
{
    public class UpdateMessageCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateMessageCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateMessageCommand command)
        {
            var value = await _context.Messages.FindAsync(command.MessageId);

            value.Name = command.Name;
            value.Surname = command.Surname;
            value.Email = command.Email;
            value.Phone = command.Phone;
            value.Subject = command.Subject;
            value.MessageDeatil = command.MessageDeatil;

            await _context.SaveChangesAsync();
        }
    }
}
