namespace CQRS_RentACar.CQRSPattern.Queries.TestimonialQueries
{
    public class GetTestimonialByIdQuery
    {
        public int TestimonialId { get; set; }

        public GetTestimonialByIdQuery(int testimonialId)
        {
            TestimonialId = testimonialId;
        }
    }
}
