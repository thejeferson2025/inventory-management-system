namespace Transactions.Dtos
{
    
    public record TransactionCreateDto(
        string Type,      
        Guid ProductId,   
        int Quantity,     
        string? Detail   
    );


    public record TransactionResponseDto(
        Guid Id,
        DateTime Date,
        string Type,
        Guid ProductId,
        int Quantity,
        decimal UnitPrice,
        decimal TotalPrice,
        string? Detail
    );


    public record TransactionUpdateDto(
        string Type,      
        Guid ProductId,   
        int Quantity,     
        string? Detail
    );
}