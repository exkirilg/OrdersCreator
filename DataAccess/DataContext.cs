using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    public DataContext(
        DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Provider> Providers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrdersItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasIndex(nameof(Order.Number), $"{nameof(Order.Provider)}Id")
            .IsUnique();
    }
}
