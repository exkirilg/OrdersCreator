namespace Domain.DTO;

public record GetProvidersRequest(
    int? Limit,
    int? Offset);
