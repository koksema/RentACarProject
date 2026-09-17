namespace CQRS_RentACar.CQRSPattern.Commands.ServicesCommands
{
    public class RemoveServicesCommand
    {
        public int ServicesId { get; set; }

        public RemoveServicesCommand(int serviceId)
        {
            ServicesId = serviceId;
        }
    }
}
