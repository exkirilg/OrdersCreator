using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record NewProviderRequest(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")] string Name);
