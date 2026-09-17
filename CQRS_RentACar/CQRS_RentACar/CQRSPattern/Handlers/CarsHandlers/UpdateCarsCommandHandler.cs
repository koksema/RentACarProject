using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.CarsCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers
{
    public class UpdateCarsCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateCarsCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateCarsCommand command)
        {
            var value = await _context.Cars.FindAsync(command.CarsId);

            value.CarsImg = command.CarsImg;
            value.Brand = command.Brand;
            value.Model = command.Model;
            value.Review = command.Review;
            value.Price = command.Price;
            value.Seat = command.Seat;
            value.Transmission = command.Transmission;
            value.Fuel = command.Fuel;
            value.Year = command.Year;
            value.Km = command.Km;

            await _context.SaveChangesAsync();
        }

    }
}