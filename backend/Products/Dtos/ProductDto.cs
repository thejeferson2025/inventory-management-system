namespace Products.Dtos
{
    // DTO para devolver información al cliente 
    public record ProductResponseDto(
        Guid Id,
        string Name,
        string Description,
        string Category,
        string ImageUrl,
        decimal Price,
        int Stock
    );

    // DTO para crear/actualizar 
    public record CreateUpdateProductDto(
        string Name,
        string Description,
        string Category,
        string ImageUrl,
        decimal Price,
        int Stock
    );
}