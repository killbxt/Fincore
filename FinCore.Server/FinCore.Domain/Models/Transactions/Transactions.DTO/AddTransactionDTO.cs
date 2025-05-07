namespace FinCore.Domain.Models.Transactions.Transactions.DTO
{
    public class AddTransactionDTO
    {
        public string? Description { get; set; }
        public string? Merchant { get; set; }
        public required Guid CardFromId { get; set; }
        public Guid? CardToId { get; set; }
        public required Transaction.TransactionType Type { get; set; }
        public required decimal Amount { get; set; }
    }
}
