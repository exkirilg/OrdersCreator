namespace Domain.DTO;

public record GetOrdersRequest(
    DateTime? From,
    DateTime? To,
    int? Limit,
    int? Offset,
    int? ProviderId);
