namespace FinCore.Domain.Models.Cards.Cards.DTO
{
    public class AddCardDTO
    {
        public required Card.CardType Type { get; set; }
        public required string CardNumber { get; set; }
        public required string CVC { get; set; } 
        public required Guid ClientId { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
    }
}
