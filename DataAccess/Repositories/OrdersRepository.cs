using Domain.DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class OrdersRepository : GenericRepository<Order>, IOrdersRepository
{
    public OrdersRepository(DataContext context) : base(context)
    {
    }

    public async Task<int> GetOrdersNumber()
    {
        return await _dbSet.CountAsync();
    }

    public async Task RemoveItems(IEnumerable<int> itemsIds)
    {
        var itemsDbSet = _context.Set<OrderItem>();
        var items = await itemsDbSet.Where(i => itemsIds.Contains(i.Id)).ToArrayAsync();
        itemsDbSet.RemoveRange(items);
    }
}
