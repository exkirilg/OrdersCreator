using Domain.Models;

namespace Domain.DataAccess;

public interface IOrdersRepository : IGenericRepository<Order>
{
    Task<int> GetOrdersNumber();
    Task RemoveItems(IEnumerable<int> itemsIds);
}
