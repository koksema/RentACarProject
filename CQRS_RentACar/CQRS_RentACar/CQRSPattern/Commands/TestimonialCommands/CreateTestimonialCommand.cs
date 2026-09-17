namespace CQRS_RentACar.CQRSPattern.Commands.TestimonialCommands
{
    public class CreateTestimonialCommand
    {
        public string ImgUrl { get; set; }
        public string Name { get; set; }
        public string Profession { get; set; }
        public int Star { get; set; }
        public string Comment { get; set; }
    }
}
