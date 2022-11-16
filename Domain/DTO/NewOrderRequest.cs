using System.ComponentModel.DataAnnotations;

namespace Domain.DTO;

public record NewOrderRequest(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order number is required")] string Number,
    [Required(ErrorMessage = "Order date is required")] DateTime Date,
    [Required(ErrorMessage = "Provider for the order is required")] int ProviderId,
    IEnumerable<NewOrderRequestItem> Items);

public record NewOrderRequestItem(
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order item name is required")] string Name,
    float Quantity,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order item unit is required")] string Unit);
