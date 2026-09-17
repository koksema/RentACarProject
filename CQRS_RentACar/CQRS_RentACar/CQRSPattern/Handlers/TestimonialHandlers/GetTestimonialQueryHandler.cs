using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Results.TestimonialResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers
{
    public class GetTestimonialQueryHandler
    {
        private readonly DemoContext _context;

        public GetTestimonialQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<List<GetTestimonialQueryResult>> Handle()
        {
            var value = await _context.Testimonials.ToListAsync();
            return value.Select(x => new GetTestimonialQueryResult
            {
                TestimonialId = x.TestimonialId,
                ImgUrl = x.ImgUrl,
                Name = x.Name,
                Profession=x.Profession,
                Star=x.Star,
                Comment=x.Comment,

            }).ToList();

        }
    }
}
