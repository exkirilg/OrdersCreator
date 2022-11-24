﻿using Domain.Models;
using System.Linq.Expressions;

namespace Domain.DataAccess;

public interface IOrdersRepository : IGenericRepository<Order>
{
    Task<int> GetOrdersNumber(Expression<Func<Order, bool>>? filter = null);
    Task RemoveItems(IEnumerable<int> itemsIds);
}
