using FinCore.Domain.Models.Transactions;
using FinCore.Domain.Models.Transactions.Transactions.DTO;
using FinCore.Services.TransactionsService;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace FinCore.API.Controllers
{
    [ApiController]
    [Route("Fincore.API/[controller]")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpPost("MakeTransaction")]
        public async Task<IActionResult> MakeTransactionByBothNumbers(MakeTransactionDTO MTD)
        {
            try
            {
                await _transactionService.MakeTransactionByBothNumbers(MTD);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id, cancellationToken);
                return transaction == null ? NotFound() : Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении транзакции по ID.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactionsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactionsAsync(cancellationToken);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении списка транзакций.");
            }
        }

        [HttpPost("find")]
        public async Task<IActionResult> FindTransactionsAsync([FromBody] Expression<Func<Transaction, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                if (predicate == null) return BadRequest("Predicate cannot be null");
                var transactions = await _transactionService.FindTransactionsAsync(predicate, cancellationToken);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при поиске транзакций.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> MakeTransactionAsync([FromBody] AddTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (transactionDto == null) return BadRequest("Transaction DTO cannot be null");
                await _transactionService.MakeTransactionAsync(transactionDto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при создании транзакции.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionAsync(Guid id, [FromBody] UpdateTransactionDTO transactionDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (transactionDto == null) return BadRequest("Transaction DTO cannot be null");
                await _transactionService.UpdateTransactionAsync(id, transactionDto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при обновлении транзакции.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _transactionService.DeleteTransactionAsync(id, cancellationToken);
                return Ok(); 
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при удалении транзакции.");
            }
        }

        [HttpGet("card/{cardId}")]
        public async Task<IActionResult> GetTransactionsByCardIdAsync(Guid cardId, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByCardIdAsync(cardId, cancellationToken);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении транзакций по ID карты.");
            }
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetTransactionsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByClientIdAsync(clientId, cancellationToken);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении транзакций по ID клиента.");
            }
        }

        [HttpGet("client/{clientId}/total-amount")]
        public async Task<IActionResult> GetTotalAmountByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var totalAmount = await _transactionService.GetTotalAmountByClientIdAsync(clientId, cancellationToken);
                return Ok(totalAmount);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении общей суммы по ID клиента.");
            }
        }
    }
}
