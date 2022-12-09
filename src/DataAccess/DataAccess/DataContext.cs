using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public abstract class DataContext<T> : DbContext, IDataContext
    where T : DbContext
{
    public DataContext(
        DbContextOptions<T> options) : base(options)
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
