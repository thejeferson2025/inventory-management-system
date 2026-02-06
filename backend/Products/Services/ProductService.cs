using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Dtos;
using Products.Models;

namespace Products.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _context.Products.ToListAsync();
            return products.Select(p => new ProductResponseDto(p.Id, p.Name, p.Description, p.Category, p.ImageUrl, p.Price, p.Stock));
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            return new ProductResponseDto(product.Id, product.Name, product.Description, product.Category, product.ImageUrl, product.Price, product.Stock);
        }

        public async Task<ProductResponseDto> CreateAsync(CreateUpdateProductDto dto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(), 
                Name = dto.Name,
                Description = dto.Description,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                Price = dto.Price,
                Stock = dto.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return new ProductResponseDto(product.Id, product.Name, product.Description, product.Category, product.ImageUrl, product.Price, product.Stock);
        }

        public async Task<bool> UpdateAsync(Guid id, CreateUpdateProductDto dto)
        {
            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null) return false;

            // Actualizamos campos
            existingProduct.Name = dto.Name;
            existingProduct.Description = dto.Description;
            existingProduct.Category = dto.Category;
            existingProduct.ImageUrl = dto.ImageUrl;
            existingProduct.Price = dto.Price;
            existingProduct.Stock = dto.Stock;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}