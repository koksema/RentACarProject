using CQRS_RentACar.Context;
using CQRS_RentACar.CQRSPattern.Queries.TestimonialQueries;
using CQRS_RentACar.CQRSPattern.Results.TestimonialResults;
using Microsoft.EntityFrameworkCore;

namespace CQRS_RentACar.CQRSPattern.Handlers.TestimonialHandlers
{
    public class GetTestimonialByIdQueryHandler
    {
        private readonly DemoContext _context;

        public GetTestimonialByIdQueryHandler(DemoContext context)
        {
            _context = context;
        }
        public async Task<GetTestimonialByIdQueryResult> Handle(GetTestimonialByIdQuery query)
        {
            var value = await _context.Testimonials.FirstOrDefaultAsync(x => x.TestimonialId == query.TestimonialId);//Veritabanındaki Testimonials tablosuna bak; öyle bir satır (x) bul ki,
            //o satırın TestimonialId değeri benim sana dışarıdan gönderdiğim query.TestimonialId değerine eşit olsun.
            return new GetTestimonialByIdQueryResult
            {
                TestimonialId = value.TestimonialId,
                ImgUrl = value.ImgUrl,
                Name = value.Name,
                Profession = value.Profession,
                Star = value.Star,
                Comment = value.Comment,

            };

        }
    }
}
