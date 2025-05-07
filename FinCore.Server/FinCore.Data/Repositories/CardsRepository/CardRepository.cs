using FinCore.Data.Context;
using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Cards.Cards.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.CardsRepository
{
    public class CardRepository : ICardRepository
    {
        private readonly ILogger<CardRepository> _logger;
        private readonly FinCoreDbContext _finCoreDbContext;

        public CardRepository(ILogger<CardRepository> logger, FinCoreDbContext finCoreDbContext)
        {
            _logger = logger;
            _finCoreDbContext = finCoreDbContext;
        }


        private static string GenerateCvv(int length = 3)
        {
            Random random = new Random();
            string cvv = "";

            for (int i = 0; i < length; i++)
            {
                cvv += random.Next(0, 10).ToString();
            }

            return cvv;
        }

        private static int CalculateLuhnCheckDigit(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                alternate = !alternate;
            }

            int checkDigit = (10 - (sum % 10)) % 10;
            return checkDigit;
        }

        public static string GenerateCreditCardNumber(string prefix = "4276", int length = 16)
        {
            Random random = new Random();
            string cardNumber = prefix;

            while (cardNumber.Length < length - 1)
            {
                cardNumber += random.Next(0, 10).ToString();
            }

         
            int checkDigit = CalculateLuhnCheckDigit(cardNumber);
            cardNumber += checkDigit.ToString();

            return cardNumber;
        }

        public async Task AddAsync(AddCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var card = (Card)cardDto;
                DateTime expireDateTime = new DateTime(2028,DateTime.Now.Month, DateTime.Now.Day);
                card.ExpiryDate = DateOnly.FromDateTime(expireDateTime);
                card.CVC = GenerateCvv();
                card.CardNumber = GenerateCreditCardNumber();
                await _finCoreDbContext.Cards.AddAsync(card, cancellationToken);
                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error adding card.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding card.");
            }
        }

        public async Task DeleteAsync(Card card, CancellationToken cancellationToken = default)
        {
            try
            {
                _finCoreDbContext.Cards.Remove(card);
                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting card.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting card.");
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var card = await _finCoreDbContext.Cards.FindAsync(id, cancellationToken);
                if (card != null)
                {
                    await DeleteAsync(card, cancellationToken);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error deleting card by ID.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting card by ID.");
            }

        }

        public async Task<List<CardDTO>> FindAsync(Expression<Func<Card, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Cards
                    .Where(predicate)
                    .Select(c => (CardDTO)c) 
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding cards.");
                throw;
            }
        }

        public async Task<List<CardDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Cards
                    .Select(c => (CardDTO)c)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all cards.");
                throw;
            }
        }

        public async Task<CardDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                return (CardDTO?)await _finCoreDbContext.Cards.FindAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting card by ID.");
                throw;
            }
        }

        public async Task<List<CardDTO>> GetCardsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Cards
                    .Where(c => c.ClientId == clientId)
                    .Select(c => (CardDTO)c)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cards by client ID.");
                throw;
            }
        }

        public async Task<List<CardDTO>> GetExpiredCardsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                return await _finCoreDbContext.Cards
                    .Where(c => c.ExpiryDate < today)
                    .Select(c => (CardDTO)c)
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting expired cards.");
                throw;
            }
        }

        public async Task<bool> IsCardNumberUniqueAsync(string cardNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _finCoreDbContext.Cards.AnyAsync(c => c.CardNumber == cardNumber, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking card number uniqueness.");
                throw;
            }
        }

        public async Task UpdateAsync(Guid id, UpdateCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var card = await _finCoreDbContext.Cards.FindAsync(id, cancellationToken);
                if (card == null)
                {
                    throw new KeyNotFoundException($"Card with ID {id} not found.");
                }
                card.Status = cardDto.Status ?? card.Status;
                card.Type = cardDto.Type ?? card.Type;
                card.ExpiryDate = cardDto.ExpiryDate ?? card.ExpiryDate;

                _finCoreDbContext.Update(card);
                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating card.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating card.");
                throw;
            }
        }
    }
}
