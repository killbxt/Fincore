using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Cards.Cards.DTO;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.CardsRepository
{
    public interface ICardRepository
    {
        Task<CardDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CardDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<CardDTO>> FindAsync(Expression<Func<Card, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(AddCardDTO card, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, UpdateCardDTO card, CancellationToken cancellationToken = default); 
        Task DeleteAsync(Card card, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<CardDTO>> GetCardsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<bool> IsCardNumberUniqueAsync(string cardNumber, CancellationToken cancellationToken = default);
        Task<List<CardDTO>> GetExpiredCardsAsync(CancellationToken cancellationToken = default);
    }
}
