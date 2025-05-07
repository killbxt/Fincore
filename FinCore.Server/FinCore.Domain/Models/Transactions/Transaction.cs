using FinCore.Domain.Models.Cards;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using System.ComponentModel.DataAnnotations;

namespace FinCore.Domain.Models.Transactions
{
    public class Transaction
    {
        public enum TransactionStatus 
        {
            InProccess,
            Declined,
            Done
        }

        public enum TransactionType
        {
            Purchase,
            Withdrawal, //снятия с банкомата
            Deposit, //внос через банкомат
            Transfer,
            Payment
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Merchant { get; set; }  

        public Guid CardFromId { get; set; } 

        public Card? CardFrom { get; set; }

        public Guid? CardToId { get; set; }

        public Card? CardTo { get; set; }

        public TransactionStatus Status { get; set; }

        public TransactionType Type { get; set; }

        public required decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public bool IsDeleted { get; set; } = false;

        public static explicit operator TransactionDTO(Transaction transaction)
        {
            return new TransactionDTO
            {
                Id = transaction.Id,
                Description = transaction.Description,
                Merchant = transaction.Merchant,
                CardFromId = transaction.CardFromId,
                CardToId = transaction.CardToId,
                Status = transaction.Status,
                Type = transaction.Type,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                IsDeleted = transaction.IsDeleted
            };
        }

        public static explicit operator Transaction(AddTransactionDTO transactionDto)
        {
            return new Transaction
            {
                Description = transactionDto.Description,
                Merchant = transactionDto.Merchant,
                CardFromId = transactionDto.CardFromId,
                CardToId = transactionDto.CardToId,
                Type = transactionDto.Type,
                Amount = transactionDto.Amount,
                TransactionDate = DateTime.Now // Set the transaction date automatically
            };
        }

        public static explicit operator Transaction(TransactionDTO dto)
        {
            return new Transaction
            {
                Id = dto.Id,
                Description = dto.Description,
                Merchant = dto.Merchant,
                CardFromId = dto.CardFromId,
                CardToId = dto.CardToId,
                Status = dto.Status,
                Type = dto.Type,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                IsDeleted = dto.IsDeleted
            };
        }

    }
}
