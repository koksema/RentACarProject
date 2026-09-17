using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.SliderCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers
{
    public class UpdateSliderCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateSliderCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateSliderCommand command)
        {
            var value = await _context.Sliders.FindAsync(command.SliderId);

            value.Title = command.Title;
            value.ImgUrl = command.ImgUrl;

            await _context.SaveChangesAsync();
        }
    }
}
