using Transactions.Data;
using Transactions.Services; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Base de Datos
builder.Services.AddDbContext<TransactionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Configuración del Cliente HTTP
builder.Services.AddHttpClient("ProductsClient", client =>
{
    var productsUrl = builder.Configuration["Services:ProductsUrl"];
    if (!string.IsNullOrEmpty(productsUrl))
    {
        client.BaseAddress = new Uri(productsUrl);
    }
});

// Inyección de Dependencias
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); 
app.UseAuthorization();
app.MapControllers();

app.Run();