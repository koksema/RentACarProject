namespace CQRS_RentACar.CQRSPattern.Commands.SliderCommands
{
    public class RemoveSliderCommand
    {
        public int SliderId { get; set; }

        public RemoveSliderCommand(int sliderId)
        {
            SliderId = sliderId;
        }
    }
}
