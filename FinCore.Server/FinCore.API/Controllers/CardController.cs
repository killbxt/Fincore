using FinCore.Domain.Models.Cards.Cards.DTO;
using FinCore.Domain.Models.Cards;
using FinCore.Services.CardsService;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace FinCore.API.Controllers
{
    [ApiController]
    [Route("Fincore.API/[controller]")]
    public class CardController : Controller
    {
        private readonly ICardService _cardService;
        private readonly ILogger<CardController> _logger;

        public CardController(ICardService cardService, ILogger<CardController> logger)
        {
            _cardService = cardService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCardAsync([FromBody] AddCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cardService.CreateCardAsync(cardDto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при создании карты.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var card = await _cardService.GetCardByIdAsync(id, cancellationToken);
                return card == null ? NotFound() : Ok(card);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении карты по ID: {id}", id);
                return BadRequest("Произошла ошибка при получении карты.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCardsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var cards = await _cardService.GetAllCardsAsync(cancellationToken);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении списка карт.");
            }
        }

        [HttpPost("find")]
        public async Task<IActionResult> FindCardsAsync([FromBody] Expression<Func<Card, bool>> predicate, CancellationToken cancellationToken = default)
        {
            try
            {
                if (predicate == null) return BadRequest("Predicate cannot be null");
                var cards = await _cardService.FindCardsAsync(predicate, cancellationToken);
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при поиске карт.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCardAsync(Guid id, [FromBody] UpdateCardDTO cardDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cardDto == null) return BadRequest("Card DTO cannot be null");

                await _cardService.UpdateCardAsync(id, cardDto, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при обновлении карты.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _cardService.DeleteCardAsync(id, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при удалении карты.");
            }
        }

        [HttpGet("client/{clientId}")]
        public async Task<IActionResult> GetCardsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            try
            {
                var cards = await _cardService.GetCardsByClientIdAsync(clientId, cancellationToken);
                if (cards == null)
                {
                    _logger.LogError("null в контроллере");
                }
                return Ok(cards);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении карт по ID клиента.");
            }
        }

        [HttpGet("is-card-number-unique/{cardNumber}")]
        public async Task<IActionResult> IsCardNumberUniqueAsync(string cardNumber, CancellationToken cancellationToken = default)
        {
            try
            {
                var isUnique = await _cardService.IsCardNumberUniqueAsync(cardNumber, cancellationToken);
                return Ok(isUnique);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при проверке уникальности номера карты.");
            }
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpiredCardsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var expiredCards = await _cardService.GetExpiredCardsAsync(cancellationToken);
                return Ok(expiredCards);
            }
            catch (Exception ex)
            {
                return BadRequest("Произошла ошибка при получении просроченных карт.");
            }
        }
    }
}

