using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.ServicesCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.ServicesHandlers
{
    public class CreateServicesCommandHandler
    {
        private readonly DemoContext _context;

        public CreateServicesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateServicesCommand command)
        {
            _context.Services.Add(new Services
            {
                Title = command.Title,
                Description = command.Description,
                IconUrl = command.IconUrl,

            });
            await _context.SaveChangesAsync();
        }
    }
}
