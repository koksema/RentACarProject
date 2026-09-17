namespace CQRS_RentACar.CQRSPattern.Commands.AboutCommands
{
    public class UpdateAboutCommand
    {
        public int AboutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
