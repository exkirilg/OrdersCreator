using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Order : Entity
{
    private string _number = string.Empty;
    private DateTime _date;
    private Provider _provider = null!;
    private List<OrderItem> _items = new();

    [Required]
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

            _number = value;
        }
    }

    [Required]
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

    [Required]
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

    [Required]
    public IReadOnlyCollection<OrderItem> Items
    {
        get => _items;
        private set
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
}
