namespace sep.backend.v1.Data.Entities
{
    public class TimeTableSlotSubject : BaseEntity
    {
        public int TimeTableId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubjectId { get; set; }
        public int DayOfWeek { get; set; }
        public virtual TimeTable? TimeTable { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}
