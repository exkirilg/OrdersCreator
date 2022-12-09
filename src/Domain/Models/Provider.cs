using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Provider : Entity
{
    private string _name = string.Empty;

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
                    "Provider name must not be empty",
                    nameof(value));
            }

            _name = value.Trim();
        }
    }

    public Provider()
    {
    }

    public Provider(string name)
    {
        Name = name;
    }

    public Provider Copy()
    {
        return (Provider)MemberwiseClone();
    }
}
