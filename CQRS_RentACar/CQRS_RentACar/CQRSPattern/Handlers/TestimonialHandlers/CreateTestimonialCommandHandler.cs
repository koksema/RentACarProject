using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands;
using CQRS_RentACar.Entities;

namespace CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers
{
    public class CreateTestimonialCommandHandler
    {
        private readonly DemoContext _context;

        public CreateTestimonialCommandHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task Handle(CreateTestimonialCommand command)
        {
            _context.Testimonials.Add(new Testimonial
            {
                ImgUrl = command.ImgUrl,
                Name = command.Name,
                Profession = command.Profession,
                Star = command.Star,
                Comment = command.Comment,

            });
            await _context.SaveChangesAsync();
        }
    }
}
