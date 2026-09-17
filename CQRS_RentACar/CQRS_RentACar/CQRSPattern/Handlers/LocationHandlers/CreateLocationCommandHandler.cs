using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.LocationCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers
{
    public class CreateLocationCommandHandler
    {
        private readonly DemoContext _context;

        public CreateLocationCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateLocationCommand command)
        {
            _context.Locations.Add(new Location
            {
                Name =command.Name,
                Iata = command.Iata,
                Iaco =command.Iaco,
                Latitude =command.Latitude,
                Longitude = command.Longitude,
                Country =command.Country,
                Elevation =command.Elevation,
                Timezone = command.Timezone
            });
            await _context.SaveChangesAsync();
        }
    }
}
