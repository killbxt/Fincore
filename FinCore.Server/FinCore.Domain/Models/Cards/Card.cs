using FinCore.Domain.Models.Cards.Cards.DTO;
using FinCore.Domain.Models.Clients;
using FinCore.Domain.Models.Transactions;
using System.ComponentModel.DataAnnotations;

namespace FinCore.Domain.Models.Cards
{
    public class Card
    {
        public enum CardType
        {
            Debit,
            Credit,
            Saving
        }

        public enum CardStatus
        {
            Active,
            Blocked,
            Lost,
            Expired
        }

        public required CardType Type { get; set; }

        public required CardStatus Status { get; set; } = CardStatus.Active;

        [Key]
        public Guid Id { get; set; }

        [StringLength(16)]
        public required string CardNumber { get; set; }

        [StringLength(3)]
        public required string CVC { get; set; }

        public required Guid ClientId { get; set; }

        public Client Client { get; set; }

        public decimal Amount { get; set; }

        public DateOnly IssueDate { get; set; } // Дата выпуска карты

        public DateOnly ExpiryDate { get; set; } // Дата окончания срока действия

        public ICollection<Transaction>? Transactions { get; set; } = new List<Transaction>();

        public bool IsDeleted { get; set; } = false;

        public static explicit operator CardDTO(Card card)
        {
            return new CardDTO
            {
                Id = card.Id,
                Type = card.Type,
                Status = card.Status,
                CardNumber = card.CardNumber,   
                ClientId = card.ClientId,
                IssueDate = card.IssueDate,
                ExpiryDate = card.ExpiryDate,
                Amount = card.Amount
            };
        }

        public static explicit operator Card(AddCardDTO addCardDto)
        {
            return new Card
            {
                Status = CardStatus.Active,
                Type = addCardDto.Type,
                CardNumber = addCardDto.CardNumber,
                CVC = addCardDto.CVC,
                ClientId = addCardDto.ClientId,
                IssueDate = addCardDto.IssueDate,
                ExpiryDate = addCardDto.ExpiryDate,
            };
        }
    }
}
