using FinCore.Domain.Models.Cards.Cards.DTO;
using FinCore.Domain.Models.Cards;
using System.Linq.Expressions;

namespace FinCore.Services.CardsService
{
    public interface ICardService
    {
        Task CreateCardAsync(AddCardDTO cardDto, CancellationToken cancellationToken = default);
        Task<CardDTO?> GetCardByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CardDTO>> GetAllCardsAsync(CancellationToken cancellationToken = default);
        Task<List<CardDTO>> FindCardsAsync(Expression<Func<Card, bool>> predicate, CancellationToken cancellationToken = default);
        Task UpdateCardAsync(Guid id, UpdateCardDTO cardDto, CancellationToken cancellationToken = default); 
        Task DeleteCardAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<CardDTO>> GetCardsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<bool> IsCardNumberUniqueAsync(string cardNumber, CancellationToken cancellationToken = default);
        Task<List<CardDTO>> GetExpiredCardsAsync(CancellationToken cancellationToken = default);
    }
}
