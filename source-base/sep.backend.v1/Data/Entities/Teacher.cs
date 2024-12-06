namespace sep.backend.v1.Data.Entities
{
    public class Teacher : BaseUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int SchoolId { get; set; }
        public bool? IsCompleteVerify { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<TeacherSubject>? TeacherSubjects { get; set; }
        public virtual ICollection<ClassEnrollment>? ClassEnrollments { get; set; }
        public virtual ICollection<ClubEnrollment>? ClubEnrollments { get; set; }
    }
}