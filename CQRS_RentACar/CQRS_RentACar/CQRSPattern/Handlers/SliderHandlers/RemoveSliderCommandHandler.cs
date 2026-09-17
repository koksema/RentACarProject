using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.SliderCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.SliderHandlers
{
    public class RemoveSliderCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveSliderCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveSliderCommand command)
        {
            var value = await _context.Sliders.FindAsync(command.SliderId);
            _context.Sliders.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
