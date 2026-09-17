namespace CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands
{
    public class RemoveTestimonialCommand
    {
        public int TestimonialId { get; set; }

        public RemoveTestimonialCommand(int testimanialId)
        {
            TestimonialId = testimanialId;
        }
    }
}
