using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers
{
    public class RemoveFeaturesCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveFeaturesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveFeaturesCommand command)
        {
            var value = await _context.Features.FindAsync(command.FeaturesId);
            _context.Features.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
