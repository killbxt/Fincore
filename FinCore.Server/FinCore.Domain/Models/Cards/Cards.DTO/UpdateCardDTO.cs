namespace FinCore.Domain.Models.Cards.Cards.DTO
{
    public class UpdateCardDTO
    {
        public Guid Id { get; set; }
        public Card.CardType? Type { get; set; }
        public Card.CardStatus? Status { get; set; }
        public DateOnly? ExpiryDate { get; set; }
        public decimal Amount { get; set; }
    }
}
