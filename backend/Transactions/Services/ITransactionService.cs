using Transactions.Dtos;

namespace Transactions.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponseDto>> GetAllAsync();
        Task<TransactionResponseDto> CreateAsync(TransactionCreateDto dto);
        Task<bool> UpdateAsync(Guid id, TransactionUpdateDto dto); 
        Task<bool> DeleteAsync(Guid id); 
    }
}
