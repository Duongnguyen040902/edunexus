namespace sep.backend.v1.Data.Entities
{
    public class PupilScore : BaseEntity
    {
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public float Score { get; set; }
        public int SemesterId { get; set; }
        public int Status { get; set; }

        public virtual Subject? Subject { get; set; }
        public virtual Pupil? Pupil { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
