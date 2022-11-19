namespace Domain.Models;

public class Provider : Entity
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (value == Name) return;

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Provider name must not be empty",
                    nameof(value));
            }

            _name = value;
        }
    }

    public Provider()
    {
    }

    public Provider(string name)
    {
        Name = name;
    }
}
