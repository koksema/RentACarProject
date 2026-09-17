using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.BookingCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class RemoveBookingCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveBookingCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveBookingCommand command)
        {
            var value = await _context.Bookings.FindAsync(command.BookingId);
            _context.Bookings.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
