namespace CQRS_RentACar.CQRSPattern.Commands.MessageCommands
{
    public class CreateMessageCommand
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string MessageDeatil { get; set; }
    }
}
