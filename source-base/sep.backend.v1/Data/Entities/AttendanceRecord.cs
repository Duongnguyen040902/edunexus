namespace sep.backend.v1.Data.Entities
{
    public class AttendanceRecord : BaseEntity
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public int? ClassId { get; set; }
        public int? ClubId { get; set; }
        public int? BusId { get; set; }
        public bool? IsAttend { get; set; }
        public int AttendanceSession { get; set; }
        public int AttendanceType { get; set; }
        public string? Feedback { get; set; }
        public virtual Pupil? Pupil { get; set; }
        public virtual Class? Class { get; set; }
        public virtual Club? Club { get; set; }
        public virtual Bus? Bus { get; set; }
    }
}
