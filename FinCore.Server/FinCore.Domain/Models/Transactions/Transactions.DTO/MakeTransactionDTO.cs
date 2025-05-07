namespace FinCore.Domain.Models.Transactions.Transactions.DTO
{
    public class MakeTransactionDTO
    {
        public string? Description { get; set; }
        public string? Merchant { get; set; }
        public required string CardFromNumber { get; set; }
        public string? CardToNumber { get; set; }
        public required Transaction.TransactionType Type { get; set; }
        public required decimal Amount { get; set; }
    }
}
