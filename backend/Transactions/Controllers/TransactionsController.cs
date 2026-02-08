using Transactions.Dtos;
using Transactions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponseDto>>> GetAll()
        {
            var transactions = await _service.GetAllAsync();
            return Ok(transactions);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponseDto>> Create(TransactionCreateDto dto)
        {
            try
            {
                var transaction = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetAll), new { id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
              
                return BadRequest(new { message = ex.Message });
            }
        }
   
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, TransactionUpdateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Transacción no encontrada" });
            return NoContent();
        }

     
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Transacción no encontrada para eliminar" });
            return NoContent();
        }
   
   
   
    }
}