using Domain.Models;

namespace Domain.DTO.Responses;

public record GetProvidersResponse(
    IEnumerable<Provider> Providers,
    int ProvidersNumber);
