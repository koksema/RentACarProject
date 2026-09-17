using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.ServicesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers
{
    public class RemoveServicesCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveServicesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveServicesCommand command)
        {
            var value = await _context.Services.FindAsync(command.ServicesId);
            _context.Services.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
