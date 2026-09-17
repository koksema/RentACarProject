using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.BookingCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class UpdateBookingCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateBookingCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateBookingCommand command)
        {
            var value = await _context.Bookings.FindAsync(command.BookingId);

            value.PickUpLocation = command.PickUpLocation;
            value.DropOffLocation = command.DropOffLocation;
            value.PickUpDate = command.PickUpDate;
            value.DropOffDate = command.DropOffDate;
            value.CarsId=command.CarId;

            await _context.SaveChangesAsync();
        }
    }
}
