using System.Reflection.Metadata;
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers
{
    public class RemoveAboutCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveAboutCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveAboutCommand command)
        {
            var value = await _context.Abouts.FindAsync(command.AboutId);
            _context.Abouts.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
