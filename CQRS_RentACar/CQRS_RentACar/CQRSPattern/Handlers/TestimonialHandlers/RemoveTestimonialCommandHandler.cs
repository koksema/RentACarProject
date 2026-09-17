using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers
{
    public class RemoveTestimonialCommandHandler
    {
        private readonly DemoContext _context;

        public RemoveTestimonialCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(RemoveTestimonialCommand command)
        {
            var value = await _context.Testimonials.FindAsync(command.TestimonialId);
            _context.Testimonials.Remove(value);
            await _context.SaveChangesAsync();
        }
    }
}
