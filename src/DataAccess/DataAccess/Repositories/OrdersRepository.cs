using Domain.DataAccessInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories;

public class OrdersRepository<TDataContext> : GenericRepository<Order, TDataContext>, IOrdersRepository
    where TDataContext : DbContext, IDataContext
{
    public OrdersRepository(TDataContext context) : base(context)
    {
    }

    public async Task<int> GetOrdersNumber(Expression<Func<Order, bool>>? filter = null)
    {
        IQueryable<Order> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        return await query.CountAsync();
    }

    public async Task RemoveItems(IEnumerable<int> itemsIds)
    {
        var itemsDbSet = _context.Set<OrderItem>();
        var items = await itemsDbSet.Where(i => itemsIds.Contains(i.Id)).ToArrayAsync();
        itemsDbSet.RemoveRange(items);
    }
}
