using Domain.Models;

namespace Domain.DTO.Responses;

public record GetOrdersResponse(
    IEnumerable<Order> Orders,
    int OrdersNumber);
