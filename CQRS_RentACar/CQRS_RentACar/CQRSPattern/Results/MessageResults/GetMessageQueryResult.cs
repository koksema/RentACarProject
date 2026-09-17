namespace CQRS_RentACar.CQRSPattern.Results.MessageResults
{
    public class GetMessageQueryResult
    {
        public int MessageId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string MessageDeatil { get; set; }


        public string? AiReply { get; set; }

        public DateTime MessageDate { get; set; }

        public bool IsRead { get; set; }
    }
}