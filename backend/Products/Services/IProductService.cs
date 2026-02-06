using Products.Dtos;

namespace Products.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();
        Task<ProductResponseDto?> GetByIdAsync(Guid id);
        Task<ProductResponseDto> CreateAsync(CreateUpdateProductDto productDto);
        Task<bool> UpdateAsync(Guid id, CreateUpdateProductDto productDto);
        Task<bool> DeleteAsync(Guid id);
    }
}