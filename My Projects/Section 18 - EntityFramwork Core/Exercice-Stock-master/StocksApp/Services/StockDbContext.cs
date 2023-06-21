using Microsoft.EntityFrameworkCore;
using StocksApp.Models;

public class StockDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }

    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Order>().ToTable("Orders");
    }
}