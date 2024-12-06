namespace sep.backend.v1.Data.Entities
{
    public class Permission : BaseEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string PermissionName { get; set; }
        public Role Role { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
