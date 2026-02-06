using Microsoft.AspNetCore.Mvc;
using Products.Dtos;
using Products.Services;

namespace Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound(new { message = "Producto no encontrado" });
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> Create(CreateUpdateProductDto dto)
        {
            var newProduct = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, CreateUpdateProductDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (!updated) return NotFound(new { message = "Producto no encontrado para actualizar" });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Producto no encontrado para eliminar" });
            return NoContent();
        }
    }
}