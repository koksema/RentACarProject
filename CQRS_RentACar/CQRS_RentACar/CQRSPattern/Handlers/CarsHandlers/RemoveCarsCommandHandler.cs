using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;
using CQRS_RentACar.CQRSPattern.Commands.CarsCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers
{
    public class RemoveCarsCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveCarsCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveCarsCommand command)
        {
            var value = await _context.Cars.FindAsync(command.CarsId);
            _context.Cars.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
