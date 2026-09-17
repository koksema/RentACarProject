namespace CQRS_RentACar.CQRSPattern.Commands.MessageCommands
{
    public class RemoveMessageCommand
    {
        public int MessageId { get; set; }

        public RemoveMessageCommand(int messageId)
        {
            MessageId = messageId;
        }
    }
}
