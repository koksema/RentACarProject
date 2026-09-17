namespace CQRS_RentACar.CQRSPattern.Commands.ServicesCommands
{
    public class UpdateServicesCommand
    {
        public int ServicesId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
    }
}
