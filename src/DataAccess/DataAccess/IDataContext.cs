using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IDataContext
{
    public DbSet<Provider> Providers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrdersItems { get; set; }
}
