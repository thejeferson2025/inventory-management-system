using System.Text.Json;
using Transactions.Data;
using Transactions.Dtos;
using Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace Transactions.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly TransactionDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public TransactionService(TransactionDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetAllAsync()
        {
            var transactions = await _context.Transactions.ToListAsync();
            return transactions.Select(t => new TransactionResponseDto(
                t.Id, t.Date, t.Type, t.ProductId, t.Quantity, t.UnitPrice, t.TotalPrice, t.Detail
            ));
        }

        public async Task<TransactionResponseDto> CreateAsync(TransactionCreateDto dto)
        {
            
            var client = _httpClientFactory.CreateClient("ProductsClient");

            var response = await client.GetAsync($"{dto.ProductId}");
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("El producto no existe o el servicio de productos no responde.");

            var content = await response.Content.ReadAsStringAsync();
            var productInfo = JsonSerializer.Deserialize<ProductExternalDto>(content, _jsonOptions);

            if (productInfo == null) throw new Exception("Error al leer datos del producto.");

            //  Validaciones de Negocio
            int newStock = productInfo.Stock;

            if (dto.Type.Equals("Sale", StringComparison.OrdinalIgnoreCase))
            {
                if (productInfo.Stock < dto.Quantity)
                    throw new Exception($"Stock insuficiente. Solo quedan {productInfo.Stock} unidades.");
                
                newStock -= dto.Quantity;
            }
            else if (dto.Type.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
            {
                newStock += dto.Quantity;
            }
            else
            {
                throw new Exception("Tipo de transacción inválido. Use 'Sale' o 'Purchase'.");
            }

            //  Crear Transacción 
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Type = dto.Type,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UnitPrice = productInfo.Price, 
                TotalPrice = productInfo.Price * dto.Quantity,
                Detail = dto.Detail
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // llamaada sincronica
            var updateDto = new
            {
                Name = productInfo.Name,
                Description = productInfo.Description,
                Category = productInfo.Category,
                ImageUrl = productInfo.ImageUrl,
                Price = productInfo.Price,
                Stock = newStock 
            };

            var updateResponse = await client.PutAsJsonAsync($"{dto.ProductId}", updateDto);

            if (!updateResponse.IsSuccessStatusCode)
            {

                throw new Exception("Error al actualizar el stock en el servicio de productos.");
            }

            return new TransactionResponseDto(
                transaction.Id, transaction.Date, transaction.Type, 
                transaction.ProductId, transaction.Quantity, 
                transaction.UnitPrice, transaction.TotalPrice, transaction.Detail
            );
        }
   
        public async Task<bool> UpdateAsync(Guid id, TransactionUpdateDto dto)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;


            var client = _httpClientFactory.CreateClient("ProductsClient");
            var response = await client.GetAsync($"{dto.ProductId}");
            
            if (!response.IsSuccessStatusCode)
                throw new Exception("No se pudo verificar el stock del producto.");

            var content = await response.Content.ReadAsStringAsync();
            var productInfo = JsonSerializer.Deserialize<ProductExternalDto>(content, _jsonOptions);

            if (productInfo == null) throw new Exception("Error al leer datos del producto.");

            if (dto.Type.Equals("Sale", StringComparison.OrdinalIgnoreCase))
            {
                if (productInfo.Stock < dto.Quantity)
                    throw new Exception($"Stock insuficiente para esta edición. Disponible: {productInfo.Stock}");
            }

            transaction.ProductId = dto.ProductId;
            transaction.Type = dto.Type;
            transaction.Quantity = dto.Quantity;
            transaction.Detail = dto.Detail;
            transaction.TotalPrice = transaction.UnitPrice * dto.Quantity;

            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }
   
    }

        public record ProductExternalDto(
        Guid Id, string Name, string Description, string Category, 
        string ImageUrl, decimal Price, int Stock
    );
}