namespace FinCore.Domain.Models.Transactions.Transactions.DTO
{
    public class TransactionDTO
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }
        public string? Merchant { get; set; }
        public Guid CardFromId { get; set; }
        public Guid? CardToId { get; set; }
        public Transaction.TransactionStatus Status { get; set; }
        public Transaction.TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
