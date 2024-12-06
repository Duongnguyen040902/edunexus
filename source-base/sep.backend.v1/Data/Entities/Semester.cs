namespace sep.backend.v1.Data.Entities
{
    public class Semester : BaseEntity
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        
        public int SchoolYearId { get; set; }
        public virtual SchoolYear? SchoolYear { get; set; }
        public virtual ICollection<PupilScore>? StudentScores { get; set; }
        public virtual ICollection<PupilFeedback>? StudentFeedbacks { get; set; }
        public virtual ICollection<TimeTable>? TimeTables { get; set; }
        public virtual ICollection<ClassEnrollment>? ClassEnrollments { get; set; }
        public virtual ICollection<ClubEnrollment>? ClubEnrollments { get; set; }
        public virtual ICollection<BusEnrollment>? BusEnrollments { get; set; }
        public virtual ICollection<BusRouteRegistration>? BusRouteRegistrations { get; set; }
        public virtual ICollection<ClassApplication>? ClassApplications { get; set; }
    }
}
