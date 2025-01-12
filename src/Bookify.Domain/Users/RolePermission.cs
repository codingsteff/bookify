namespace Bookify.Domain.Users;

// Honestly, we could manage this directly with ef core
public sealed class RolePermission
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
}