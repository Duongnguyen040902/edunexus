namespace sep.backend.v1.Data.Entities
{
    public class PupilFeedback : BaseEntity
    {   
        public int PupilId { get; set; }
        public int SemesterId { get; set; }    
        public string Description { get; set; }
        public int Status { get; set; }
        public virtual Pupil? Pupil { get; set; }    
        public virtual Semester? Semester { get; set; }
    }
}
