namespace FinCore.Domain.Models.Transactions.Transactions.DTO
{
    public class UpdateTransactionDTO
    {
        public Guid Id { get; set; } // Required to identify the transaction to update
        public string? Description { get; set; }
        public string? Merchant { get; set; }
        public Transaction.TransactionStatus? Status { get; set; }
    }
}
