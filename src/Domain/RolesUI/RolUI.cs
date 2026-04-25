using SharedKernel;

namespace Domain.RolesUI;

public sealed class RolUI : Entity
{
    public Guid Id { get; set; }
    public Guid RolId { get; set; }
    public string TextColor { get; set; }
    public string BackgroundColor { get; set; }
}