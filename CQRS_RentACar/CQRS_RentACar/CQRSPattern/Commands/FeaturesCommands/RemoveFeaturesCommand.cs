namespace CQRS_RentACar.CQRSPattern.Commands.FeaturesCommands
{
    public class RemoveFeaturesCommand
    {
        public int FeaturesId { get; set; }

        public RemoveFeaturesCommand(int featureId)
        {
            FeaturesId = featureId;
        }
    }
}
