namespace sep.backend.v1.Data.Entities
{
    public class Pupil : BaseUserEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int SchoolId { get; set; }
        public bool? IsCompleteVerify { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<ClassEnrollment>? PupilClasses { get; set; }
        public virtual ICollection<BusEnrollment>? BusEnrollments { get; set; }
        public virtual ICollection<AttendanceRecord>? AttendanceRecords { get; set; }
        public virtual ICollection<PupilFeedback>? PupilFeedbacks { get; set; }
        public virtual ICollection<PupilScore>? PupilScores { get; set; }
        public virtual ICollection<BusRouteRegistration>? BusRouteRegistrations { get; set; }
        public virtual ICollection<ClubEnrollment>? ClubEnrollments { get; set; }
        public virtual ICollection<ClassApplication>? ClassApplications { get; set; }
    }
}