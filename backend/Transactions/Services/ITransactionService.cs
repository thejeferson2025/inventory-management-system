using Transactions.Dtos;

namespace Transactions.Services
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionResponseDto>> GetAllAsync();
        Task<TransactionResponseDto> CreateAsync(TransactionCreateDto dto);
    }
}
