using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands;

namespace CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers
{
    public class UpdateTestimonialCommandHandler
    {
        private readonly DemoContext _context;

        public UpdateTestimonialCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(UpdateTestimonialCommand command)
        {
            var value = await _context.Testimonials.FindAsync(command.TestimonialId);

            value.ImgUrl = command.ImgUrl;
            value.Name = command.Name;
            value.Profession = command.Profession;
            value.Star= command.Star;
            value.Comment = command.Comment;

            await _context.SaveChangesAsync();
        }
    }
}
