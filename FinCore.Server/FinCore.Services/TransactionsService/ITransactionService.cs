using FinCore.Domain.Models.Transactions;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using System.Linq.Expressions;

namespace FinCore.Services.TransactionsService
{
    public interface ITransactionService
    {
        Task<TransactionDTO?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> FindTransactionsAsync(Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken = default);
        Task MakeTransactionAsync(AddTransactionDTO transactionDto, CancellationToken cancellationToken = default); //Add business logic (validation, etc.)
        Task UpdateTransactionAsync(Guid id, UpdateTransactionDTO transactionDto, CancellationToken cancellationToken = default); // Add validation and business rules.
        Task DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> MakeTransactionByBothNumbers(MakeTransactionDTO MTD);

        Task<List<TransactionDTO>> GetTransactionsByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default);
        Task<List<TransactionDTO>> GetTransactionsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<decimal> GetTotalAmountByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    }
}
