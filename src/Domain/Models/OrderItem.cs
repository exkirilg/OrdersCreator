using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class OrderItem : Entity
{
    private string _name = string.Empty;
    private float _quantity;
    private string _unit = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name must be filled")]
    [StringLength(150, ErrorMessage = "Name must not exceed 150 characters")]
    public string Name
    {
        get => _name;
        set
        {
            if (value == Name) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Order item name must not be empty",
                    nameof(value));
            }

            _name = value.Trim();
        }
    }

    [Range(minimum: 0, maximum: float.MaxValue, ErrorMessage = "Quantity must be filled")]
    public float Quantity
    {
        get => _quantity;
        set
        {
            if (value == Quantity) return;

            if (value < 0)
            {
                throw new ArgumentException(
                    $"Order item quantity is out of range ({0} - {float.MaxValue})",
                    nameof(value));
            }

            _quantity = value;
        }
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Unit must be filled")]
    [StringLength(50, ErrorMessage = "Unit must not exceed 50 characters")]
    public string Unit
    {
        get => _unit;
        set
        {
            if (value == Unit) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Order item item must not be empty",
                    nameof(value));
            }

            _unit = value.Trim();
        }
    }

    public OrderItem()
    {
    }

    public OrderItem(string name, float quantity, string unit)
    {
        Name = name;
        Quantity = quantity;
        Unit = unit;
    }

    public OrderItem Copy()
    {
        return (OrderItem)MemberwiseClone();
    }
}
