namespace sep.backend.v1.Data.Entities
{
    public class ClubEnrollment : BaseEntity
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int? PupilId { get; set; }
        public int? TeacherId { get; set; }
        public int Status { get; set; }
        public int SemesterId { get; set; }

        public virtual Club? Club { get; set; }
        public virtual Pupil? Pupil { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
