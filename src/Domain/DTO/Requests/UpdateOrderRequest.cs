using System.ComponentModel.DataAnnotations;

namespace Domain.DTO.Requests;

public record UpdateOrderRequest(
    [Required(ErrorMessage = "Id is required")] int Id,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order number is required")] string Number,
    [Required(ErrorMessage = "Order date is required")] DateTime Date,
    [Required(ErrorMessage = "Provider for the order is required")] int ProviderId,
    IEnumerable<UpdateOrderRequestItem> Items);

public record UpdateOrderRequestItem(
    int Id,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order item name is required")] string Name,
    float Quantity,
    [Required(AllowEmptyStrings = false, ErrorMessage = "Order item unit is required")] string Unit);
