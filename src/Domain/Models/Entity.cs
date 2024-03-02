namespace Domain.Models;

public abstract class Entity
{
    public int Id { get; set; }
    public Guid UniqueIdentifier { get; set; }
}