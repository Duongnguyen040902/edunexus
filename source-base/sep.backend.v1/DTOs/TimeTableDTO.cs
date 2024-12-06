using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class TimeTableDTO
    {
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public int TimeSlotId { get; set; }
        public int SubjectId { get; set; }
        public int DayOfWeek { get; set; }
    }

    public class TimeTableDetailDTO 
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int TimeSlotId { get; set; }
        public string TimeSlotName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } 
        public int DayOfWeek { get; set; }
    }

    public class CreateTimeTableDTO
    {
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public int Status { get; set; }
    }
}
