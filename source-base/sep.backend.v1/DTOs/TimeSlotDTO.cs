namespace sep.backend.v1.DTOs
{
    public class TimeSlotDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? SchoolId { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateTimeSlotDTO
    {
        public string Name { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
