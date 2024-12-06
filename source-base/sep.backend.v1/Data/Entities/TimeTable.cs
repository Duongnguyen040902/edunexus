namespace sep.backend.v1.Data.Entities
{
    public class TimeTable
    {
        public int ClassId { get; set; }        
        public int SemesterId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubjectId { get; set; }
        public int DayOfWeek { get; set; }

        public virtual Class? Class { get; set; } 
        public virtual Semester? Semester { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
        public virtual Subject? Subject { get; set; }

    }
}
