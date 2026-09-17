using System;
using System.Diagnostics.Metrics;
using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.LocationCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.LocationHandlers
{
    public class UpdateLocationCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateLocationCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateLocationCommand command)
        {
            var value = await _context.Locations.FindAsync(command.LocationID);

            value.LocationID = command.LocationID;
            value.Name = command.Name;
            value.Iata = command.Iata;
            value.Iaco = command.Iaco;
            value.Latitude = command.Latitude;
            value.Longitude = command.Longitude;
            value.Country = command.Country;
            value.Elevation = command.Elevation;
            value.Timezone = command.Timezone;

            await _context.SaveChangesAsync();

        }
    }
}
