using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.MessageCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.MessageHandlers
{
    public class RemoveMessageCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveMessageCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveMessageCommand command)
        {
            var value = await _context.Messages.FindAsync(command.MessageId);
            _context.Messages.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
