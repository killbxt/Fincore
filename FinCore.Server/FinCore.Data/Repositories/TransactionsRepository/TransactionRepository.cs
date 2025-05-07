using FinCore.Data.Context;
using FinCore.Domain.Models.Transactions;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.TransactionsRepository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ILogger<TransactionRepository> _logger;
        private readonly FinCoreDbContext _finCoreDbContext;

        public TransactionRepository(ILogger<TransactionRepository> logger, FinCoreDbContext finCoreDbContext)
        {
            _logger = logger;
            _finCoreDbContext = finCoreDbContext;
        }

        public async Task AddAsync(AddTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
            var transaction = new Transaction
            {
                CardFromId = transactionDto.CardFromId,
                CardToId = transactionDto.CardToId,
                Amount = transactionDto.Amount,
                Type = transactionDto.Type,
                Description = transactionDto.Description,
                Merchant = transactionDto.Merchant,
                Id = Guid.NewGuid(),
                TransactionDate = DateTime.UtcNow,
                Status = Transaction.TransactionStatus.InProccess,
                IsDeleted = false
            };

            await _finCoreDbContext.Transactions.AddAsync(transaction, cancellationToken);
            await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Transaction added. ID: {transaction.Id}");

        }

        public async Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            transaction.IsDeleted = true;
            _finCoreDbContext.Transactions.Update(transaction);
            await _finCoreDbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Transaction soft deleted. ID: {transaction.Id}");
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var transaction = await _finCoreDbContext.Transactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.IsDeleted = true;
                _finCoreDbContext.Transactions.Update(transaction);
                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Transaction soft deleted. ID: {id}");
            }
            else
            {
                _logger.LogWarning($"Transaction not found for deletion. ID: {id}");
            }
        }

        public async Task<List<TransactionDTO>> FindAsync(Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _finCoreDbContext.Transactions
                .Where(predicate)
                .Where(t => !t.IsDeleted)
                .Select(t => (TransactionDTO)t)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _finCoreDbContext.Transactions
                .Where(t => !t.IsDeleted)
                .Select(t => (TransactionDTO)t)
                .ToListAsync(cancellationToken);
        }

        public async Task<TransactionDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var transaction = await _finCoreDbContext.Transactions
                .Where(t => t.Id == id && !t.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
            return transaction == null ? null : (TransactionDTO)transaction;
        }

        public async Task<decimal> GetTotalAmountByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _finCoreDbContext.Transactions
                 .Where(t => t.CardFrom.ClientId == clientId && !t.IsDeleted)
                 .SumAsync(t => t.Amount, cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
        {
            return await _finCoreDbContext.Transactions
                .Where(t => t.CardFromId == cardId && !t.IsDeleted)
                .Select(t => (TransactionDTO)t)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetTransactionsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _finCoreDbContext.Transactions
                .Where(t => t.CardFrom.ClientId == clientId && !t.IsDeleted)
                .Select(t => (TransactionDTO)t)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> MakeTransactionByBothNumbers(MakeTransactionDTO MTD)
        {
            try
            {
                var Cardfrom = await _finCoreDbContext.Cards.Where(c => c.CardNumber == MTD.CardFromNumber).FirstOrDefaultAsync();
                var CardTo = await _finCoreDbContext.Cards.Where(c => c.CardNumber == MTD.CardToNumber).FirstOrDefaultAsync();
                if(Cardfrom.CardNumber!=MTD.CardFromNumber || CardTo.CardNumber!=MTD.CardToNumber)
                {
                    _logger.LogError("Ошибка при транзакции");
                    return false;
                }
                Cardfrom.Amount -= MTD.Amount;
                CardTo.Amount += MTD.Amount;
                _finCoreDbContext.Cards.Update(Cardfrom);
                _finCoreDbContext.Cards.Update(CardTo);
                await _finCoreDbContext.Transactions.AddAsync(new Transaction
                {
                    Amount = MTD.Amount,
                    TransactionDate = DateTime.Now,
                    CardFromId = Cardfrom.Id,
                    CardToId = CardTo.Id,
                    Description = MTD.Description,
                    Type = MTD.Type,
                    Merchant = MTD.Merchant,
                    Id = Guid.NewGuid(),
                    CardFrom = Cardfrom,
                    CardTo = CardTo
                });
                await _finCoreDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
           
        }

        public async Task UpdateAsync(Guid id, UpdateTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
            var transaction = await _finCoreDbContext.Transactions.FindAsync(id);
            if (transaction != null)
            {
                transaction.Description = transactionDto.Description ?? transaction.Description;
                transaction.Merchant = transactionDto.Merchant ?? transaction.Merchant;
                transaction.Status = transactionDto.Status ?? transaction.Status;

                _finCoreDbContext.Transactions.Update(transaction);
                await _finCoreDbContext.SaveChangesAsync(cancellationToken);
                _logger.LogInformation($"Transaction updated. ID: {id}");
            }
            else
            {
                _logger.LogWarning($"Transaction not found for update. ID: {id}");
            }
        }
    }
}
