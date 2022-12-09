using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Order : Entity, IValidatableObject
{
    private string _number = string.Empty;
    private DateTime _date;
    private Provider _provider = null!;
    private List<OrderItem> _items = new();

    [Required(AllowEmptyStrings = false, ErrorMessage = "Number must be filled")]
    [StringLength(25, ErrorMessage = "Number must not exceed 25 characters")]
    public string Number
    {
        get => _number;
        set
        {
            if (value == Number) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Order number must not be empty",
                    nameof(value));
            }

            _number = value.Trim();
        }
    }

    [Required(ErrorMessage = "Date must be filled")]
    public DateTime Date
    {
        get => _date;
        set
        {
            if (value == Date) return;

            ArgumentNullException.ThrowIfNull(
                nameof(value),
                nameof(value));

            _date = value;
        }
    }

    [Required(ErrorMessage = "Provider must be filled")]
    public Provider Provider
    {
        get => _provider;
        set
        {
            if (value == Provider) return;

            ArgumentNullException.ThrowIfNull(
                nameof(value),
                nameof(value));

            _provider = value;
        }
    }

    public IReadOnlyCollection<OrderItem> Items
    {
        get => _items;
        set
        {
            ArgumentNullException.ThrowIfNull(
                nameof(value),
                nameof(value));

            _items = new(value);
        }
    }

    public Order()
    {
    }

    public Order(string number, DateTime date, Provider provider, params OrderItem[] items)
    {
        Number = number;
        Date = date;
        Provider = provider;
        Items = items;
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
    }

    public void UpdateItem(int itemId, OrderItem item)
    {
        OrderItem? currentItem = _items.Find(i => i.Id == itemId);

        if (currentItem is null)
        {
            throw new ArgumentException(
                $"Order does not contain item with id {itemId}",
                nameof(itemId));
        }

        currentItem.Name = item.Name;
        currentItem.Quantity = item.Quantity;
        currentItem.Unit = item.Unit;
    }

    public void RemoveItem(int itemId)
    {
        OrderItem? currentItem = _items.Find(i => i.Id == itemId);

        if (currentItem is null)
        {
            throw new ArgumentException(
                $"Order does not contain item with id {currentItem}",
                nameof(currentItem));
        }

        _items.Remove(currentItem);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Items.Where(i => i.Name == Number).Any())
        {
            yield return new ValidationResult(
                "Order item's name must not be equal to order's number",
                new[] { nameof(Items) });
        }
    }

    public Order Copy()
    {
        Order newOrder = new(
            Number,
            Date,
            Provider,
            Items.Select(i => i.Copy()).ToArray())
        {
            Id = Id
        };

        return newOrder;
    }
}
