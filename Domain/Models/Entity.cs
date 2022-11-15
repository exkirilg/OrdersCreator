using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public abstract class Entity : IComparable<Entity>
{
    protected int _id;

    [Key]
    public int Id
    {
        get => _id;
        private set
        {
            if (value == Id) return;

            _id = value;
        }
    }

    public int CompareTo(Entity? other)
    {
        if (other is null) return 1;

        return Id.CompareTo(other.Id);
    }
}
