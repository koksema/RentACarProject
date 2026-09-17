namespace CQRS_RentACar.CQRSPattern.Commands.SliderCommands
{
    public class UpdateSliderCommand
    {
        public int SliderId { get; set; }
        public string Title { get; set; }
        public string ImgUrl { get; set; }
    }
}
