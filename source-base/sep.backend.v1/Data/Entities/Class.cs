namespace sep.backend.v1.Data.Entities
{
    public class Class: BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SchoolId { get; set; }
        public int Status { get; set; }
        public int? Block { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<Notifications>? Notifications { get; set; }
        public virtual ICollection<TimeTable>? TimeTables { get; set; }
        public virtual ICollection<AttendanceRecord>? AttendanceRecords { get; set; }
        public virtual ICollection<ClassEnrollment>? ClassEnrollments { get; set; }
      
    }
    
}
