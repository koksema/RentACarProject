using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers
{
    public class UpdateFeaturesCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateFeaturesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateFeaturesCommand command)
        {
            var value = await _context.Features.FindAsync(command.FeaturesId);

            value.Title = command.Title;
            value.Description = command.Description;
            value.IconUrl = command.IconUrl;
            value.IconTitle = command.IconTitle;
            value.IconDescription = command.IconDescription;
            value.ImgUrl = command.ImgUrl;

            await _context.SaveChangesAsync();
        }
    }
}
