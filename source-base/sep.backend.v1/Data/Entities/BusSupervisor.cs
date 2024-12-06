namespace sep.backend.v1.Data.Entities
{
    public class BusSupervisor : BaseUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public int SchoolId { get; set; }
        public bool? IsCompleteVerify { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<BusEnrollment>? BusEnrollments { get; set; }
    }
}