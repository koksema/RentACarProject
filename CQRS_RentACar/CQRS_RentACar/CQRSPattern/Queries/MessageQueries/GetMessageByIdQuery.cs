namespace CQRS_RentACar.CQRSPattern.Queries.MessageQueries
{
    public class GetMessageByIdQuery
    {
        public int MessageId { get; set; }

        public GetMessageByIdQuery(int messageId)
        {
            MessageId = messageId;
        }
    }
}
