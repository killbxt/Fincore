using FinCore.Data.Repositories.CardsRepository;
using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Cards.Cards.DTO;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Services.CardsService
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        private readonly ILogger<CardService> _logger;

        public CardService(ICardRepository cardRepository, ILogger<CardService> logger)
        {
            _cardRepository = cardRepository;
            _logger = logger;
        }

        public async Task CreateCardAsync(AddCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            await _cardRepository.AddAsync(cardDto, cancellationToken);
        }

        public async Task DeleteCardAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _cardRepository.DeleteByIdAsync(id, cancellationToken);
        }

        public async Task<List<CardDTO>> FindCardsAsync(Expression<Func<Card, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _cardRepository.FindAsync(predicate, cancellationToken);
        }

        public async Task<List<CardDTO>> GetAllCardsAsync(CancellationToken cancellationToken = default)
        {
            return await _cardRepository.GetAllAsync(cancellationToken);
        }

        public async Task<CardDTO?> GetCardByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _cardRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<CardDTO>> GetCardsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _cardRepository.GetCardsByClientIdAsync(clientId, cancellationToken);
        }

        public async Task<List<CardDTO>> GetExpiredCardsAsync(CancellationToken cancellationToken = default)
        {
            return await _cardRepository.GetExpiredCardsAsync(cancellationToken);
        }

        public async Task<bool> IsCardNumberUniqueAsync(string cardNumber, CancellationToken cancellationToken = default)
        {
            return await _cardRepository.IsCardNumberUniqueAsync(cardNumber, cancellationToken);
        }

        public async Task UpdateCardAsync(Guid id, UpdateCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            await _cardRepository.UpdateAsync(id, cardDto, cancellationToken);
        }
    }
}
