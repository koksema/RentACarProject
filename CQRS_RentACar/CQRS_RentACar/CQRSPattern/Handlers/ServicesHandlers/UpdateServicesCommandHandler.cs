using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.ServicesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers
{
    public class UpdateServicesCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateServicesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateServicesCommand command)
        {
            var value = await _context.Services.FindAsync(command.ServicesId);

            value.Title = command.Title;
            value.Description = command.Description;
            value.IconUrl = command.IconUrl;

            await _context.SaveChangesAsync();
        }
    }
}
