namespace sep.backend.v1.DTOs
{
    public class AttendanceListDTO
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int Absentees { get; set; }
        public int Classize { get; set; }
        public int? SemesterId { get; set; }
        public int? Session { get; set; }
        public int? Type { get; set; }
        public int? EntityId { get; set; }
    }
}
