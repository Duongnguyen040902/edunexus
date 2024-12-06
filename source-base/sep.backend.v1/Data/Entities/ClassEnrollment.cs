namespace sep.backend.v1.Data.Entities
{
    public class ClassEnrollment : BaseEntity
    {
        public int Id { get; set; } 
        public int ClassId { get; set; }
        public int? TeacherId { get; set; }
        public int? PupilId { get; set; }
        public int SemesterId { get; set; }
        public string? OldTeacher { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Class? Class { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Pupil? Pupil { get; set; }
    }
}
