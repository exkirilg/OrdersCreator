namespace Domain.DTO.Requests;

public record GetOrdersRequest(
    DateTime? From,
    DateTime? To,
    int? Limit,
    int? Offset,
    int? ProviderId);
