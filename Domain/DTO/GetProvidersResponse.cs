using Domain.Models;

namespace Domain.DTO;

public record GetProvidersResponse(
    IEnumerable<Provider> Providers,
    int ProvidersNumber);
