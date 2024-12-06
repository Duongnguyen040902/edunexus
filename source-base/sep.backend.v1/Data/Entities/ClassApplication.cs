namespace sep.backend.v1.Data.Entities
{
    public class ClassApplication : BaseEntity
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public int SemesterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ApplicationCategoryId { get; set; }
        public string? Response { get; set; }
        public int Status { get; set; }       
        public virtual Pupil? Pupil { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual ClassApplicationCategory? ClassApplicationCategory { get; set; }
    }
}
