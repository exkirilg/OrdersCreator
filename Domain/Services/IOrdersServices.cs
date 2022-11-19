using Domain.DTO;
using Domain.Models;

namespace Domain.Services;

public interface IOrdersServices
{
    Task<GetOrdersResponse> Get(GetOrdersRequest request);
    Task<Order> GetById(int id);
    Task Create(NewOrderRequest request);
    Task Update(UpdateOrderRequest request);
    Task Delete(int id);
}