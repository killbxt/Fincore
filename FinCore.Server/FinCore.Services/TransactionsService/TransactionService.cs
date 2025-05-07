using FinCore.Data.Repositories.TransactionsRepository;
using FinCore.Domain.Models.Transactions;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FinCore.Services.TransactionsService
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _transactionRepository.DeleteByIdAsync(id, cancellationToken);
        }

        public async Task<List<TransactionDTO>> FindTransactionsAsync(Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.FindAsync(predicate, cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.GetAllAsync(cancellationToken);
        }

        public async Task<decimal> GetTotalAmountByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.GetTotalAmountByClientIdAsync(clientId, cancellationToken);
;        }

        public async Task<TransactionDTO?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.GetTransactionsByCardIdAsync(cardId, cancellationToken);
        }

        public async Task<List<TransactionDTO>> GetTransactionsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            return await _transactionRepository.GetTransactionsByClientIdAsync(clientId, cancellationToken);
        }

        public async Task MakeTransactionAsync(AddTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
             await _transactionRepository.AddAsync(transactionDto, cancellationToken);
        }

        public async Task<bool> MakeTransactionByBothNumbers(MakeTransactionDTO MTD)
        {
            try
            {
                await _transactionRepository.MakeTransactionByBothNumbers(MTD);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
            await _transactionRepository.UpdateAsync(id, transactionDto, cancellationToken);
        }
    }
}
