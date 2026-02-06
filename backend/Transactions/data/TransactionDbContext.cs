using Transactions.Models;
using Microsoft.EntityFrameworkCore;

namespace Transactions.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
    }
}