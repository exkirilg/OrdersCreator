using Domain.Models;

namespace Domain.DTO;

public record GetOrdersResponse(
    IEnumerable<Order> Orders,
    int OrdersNumber);
