using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.FeaturesHandlers
{
    public class CreateFeaturesCommandHandler
    {
        private readonly DemoContext _context;

        public CreateFeaturesCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateFeaturesCommand command)
        {
            _context.Features.Add(new Features
            {
                Title = command.Title,
                Description = command.Description,
                IconUrl = command.IconUrl,
                IconTitle = command.IconTitle,
                IconDescription = command.IconDescription,
                ImgUrl = command.ImgUrl,
            });
            await _context.SaveChangesAsync();
        }
    }
}
