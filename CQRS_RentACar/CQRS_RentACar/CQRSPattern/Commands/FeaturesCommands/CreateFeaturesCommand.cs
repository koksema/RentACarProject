namespace CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands
{
    public class CreateFeaturesCommand
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
        public string IconTitle { get; set; }
        public string IconDescription { get; set; }
        public string ImgUrl { get; set; }
    }
}
