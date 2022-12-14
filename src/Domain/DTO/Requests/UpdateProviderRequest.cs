using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Requests;

public record UpdateProviderRequest(
    [Required(ErrorMessage = "Id is required")] int Id,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")] string Name);
