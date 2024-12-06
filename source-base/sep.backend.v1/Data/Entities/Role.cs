namespace sep.backend.v1.Data.Entities
{
    public class Role : BaseEntity
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}