using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.LocationCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers
{
    public class RemoveLocationCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveLocationCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveLocationCommand command)
        {
            var value = await _context.Locations.FindAsync(command.LocationID);
            _context.Locations.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
