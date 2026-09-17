using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.AboutHandlers
{
    public class UpdateAboutCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateAboutCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateAboutCommand command)
        {
            var value=await _context.Abouts.FindAsync(command.AboutId);

            value.Title = command.Title;
            value.Description=command.Description;

            await _context.SaveChangesAsync();
        }
    }
}
