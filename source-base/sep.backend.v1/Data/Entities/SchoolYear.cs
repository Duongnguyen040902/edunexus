namespace sep.backend.v1.Data.Entities
{
    public class SchoolYear : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } 
        public int SchoolId { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<Semester>? Semesters { get; set; }
        public virtual ICollection<Notifications>? Notifications { get; set; }  
    }
}
