using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.BookingCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.BookingHandlers
{
    public class CreateBookingCommandHandler
    {
        private readonly DemoContext _context;

        public CreateBookingCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateBookingCommand command)
        {
            _context.Bookings.Add(new Booking
            {
                PickUpLocation = command.PickUpLocation,
                DropOffLocation = command.DropOffLocation,
                PickUpDate = command.PickUpDate,
                DropOffDate = command.DropOffDate,
                CarsId = command.CarId,

            });
            await _context.SaveChangesAsync();
        }
    }
}
