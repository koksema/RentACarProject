using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers
{
    public class CreateAboutCommandHandler
    {
        private readonly DemoContext _context;

        public CreateAboutCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateAboutCommand command)
        {
            _context.Abouts.Add(new About
            {
                Title = command.Title,
                Description = command.Description,

            });
            await _context.SaveChangesAsync();
        }
    }
}
