namespace FinCore.Domain.Models.Cards.Cards.DTO
{
    public class CardDTO
    {
        public Guid Id { get; set; }
        public Card.CardType Type { get; set; }
        public Card.CardStatus Status { get; set; }
        public string CardNumber { get; set; } = null;
        public Guid ClientId { get; set; }
        public DateOnly IssueDate { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public decimal Amount { get; set; }
    }
}
