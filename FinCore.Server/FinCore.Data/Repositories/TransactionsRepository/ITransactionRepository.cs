using FinCore.Domain.Models.Transactions;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using System.Linq.Expressions;

namespace FinCore.Data.Repositories.TransactionsRepository
{
    public interface ITransactionRepository
    {
        Task<TransactionDTO?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> FindAsync(Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken = default);
        Task AddAsync(AddTransactionDTO transaction, CancellationToken cancellationToken = default); 
        Task UpdateAsync(Guid id, UpdateTransactionDTO transaction, CancellationToken cancellationToken = default); 
        Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task <bool>MakeTransactionByBothNumbers(MakeTransactionDTO MTD);

        
        Task<List<TransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> GetTransactionsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalAmountByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    }
}
