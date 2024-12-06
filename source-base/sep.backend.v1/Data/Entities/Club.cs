namespace sep.backend.v1.Data.Entities
{
    public class Club : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int SchoolId { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<ClubEnrollment>? ClubEnrollments { get; set; }
        public virtual ICollection<AttendanceRecord>? AttendanceRecords { get; set; }
    }
}
