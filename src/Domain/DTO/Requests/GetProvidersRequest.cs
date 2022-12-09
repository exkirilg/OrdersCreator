namespace Domain.DTO.Requests;

public record GetProvidersRequest(
    int? Limit,
    int? Offset);
