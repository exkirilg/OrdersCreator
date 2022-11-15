using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DataContext : DbContext
{
    public DataContext(
        DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<Provider> Providers { get; set; } = null!;
}
