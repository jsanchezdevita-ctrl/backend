namespace SharedKernel;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public bool Deleted { get; private set; } = false;
    public DateTime? DeletedAt { get; private set; }
    public string? DeletedBy { get; private set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void SoftDelete(string deletedBy = null)
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }
}
