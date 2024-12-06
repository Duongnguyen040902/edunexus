namespace sep.backend.v1.Data.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int SchoolId { get; set; }
        public virtual School? School { get; set; }
        public virtual ICollection<PupilFeedback>? PupilFeedbacks { get; set; }
        public virtual ICollection<PupilScore>? PupilScores { get; set; }
        public virtual ICollection<TimeTable>? TimeTables { get; set; }
        public virtual ICollection<TeacherSubject>? TeacherSubjects { get; set; }
    }
}
