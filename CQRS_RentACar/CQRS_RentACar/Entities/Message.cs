namespace CQRS_RentACar.Entities
{
    public class Message
    {
        public int MessageId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Subject { get; set; }

        public string MessageDeatil { get; set; }


        // Gemini tarafından oluşturulan cevap
        public string? AiReply { get; set; }


        // Mesajın gönderilme tarihi
        public DateTime MessageDate { get; set; }


        // Admin mesajı okudu mu?
        public bool IsRead { get; set; }
    }
}