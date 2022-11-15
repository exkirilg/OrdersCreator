using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class OrderItem : Entity
{
    private Order _order = null!;
    private string _name = string.Empty;
    private float _quantity;
    private string _unit = string.Empty;

    [Required]
    public Order Order
    {
        get => _order;
        set
        {
            if (value == Order) return;

            ArgumentNullException.ThrowIfNull(
                nameof(value),
                nameof(value));

            _order = value;
        }
    }

    [Required]
    public string Name
    {
        get => _name;
        private set
        {
            if (value == Name) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Order item name must not be empty",
                    nameof(value));
            }

            _name = value;
        }
    }

    [Range(minimum: 0, maximum: float.MaxValue)]
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

    [Required]
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

            _unit = value;
        }
    }

    private OrderItem()
    {
    }

    public OrderItem(Order order, string name, float quantity, string unit)
    {
        Order = order;
        Name = name;
        Quantity = quantity;
        Unit = unit;
    }
}
