namespace sep.backend.v1.Data.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }
        public int? SchoolId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual School? School { get; set; }
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    }
}