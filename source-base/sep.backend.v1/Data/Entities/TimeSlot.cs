namespace sep.backend.v1.Data.Entities
{
    public class TimeSlot : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? SchoolId { get; set; }
        public bool IsActive { get; set; }
        public virtual School? School { get; set; }
       

        public virtual ICollection<TimeTable>? TimeTables { get; set; }     
    }
}
