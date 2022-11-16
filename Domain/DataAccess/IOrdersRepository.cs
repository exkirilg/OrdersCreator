using Domain.Models;

namespace Domain.DataAccess;

public interface IOrdersRepository : IGenericRepository<Order>
{
    Task RemoveItems(IEnumerable<int> itemsIds);
}
