using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.SliderCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.SlidersHandlers
{
    public class CreateSliderCommandHandler
    {
        private readonly DemoContext _context;

        public CreateSliderCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateSliderCommand command)
        {
            _context.Sliders.Add(new Slider
            {
                Title = command.Title,
                ImgUrl = command.ImgUrl,

            });
            await _context.SaveChangesAsync();
        }
    }
}
