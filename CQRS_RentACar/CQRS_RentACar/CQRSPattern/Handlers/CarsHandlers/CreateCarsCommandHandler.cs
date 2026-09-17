using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.AboutCommands;
using CQRS_RentACar.CQRSPattern.Commands.CarsCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.CarsHandlers
{
    public class CreateCarsCommandHandler
    {
        private readonly DemoContext _context;

        public CreateCarsCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateCarsCommand command)
        {
            _context.Cars.Add(new Cars
            {
               CarsImg=command.CarsImg,
               Brand=command.Brand,
               Model=command.Model,
               Review=command.Review,
               Price=command.Price,
               Seat=command.Seat,
               Transmission=command.Transmission,
               Fuel=command.Fuel,
               Year=command.Year,
               Km=command.Km,

            });
            await _context.SaveChangesAsync();
        }
    }
}
