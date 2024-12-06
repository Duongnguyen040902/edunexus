namespace sep.backend.v1.DTOs
{
    public class ClubDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int SchoolId { get; set; }
    }

    public class ClubDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int? SchoolYearId { get; set; }
        public string? SchoolYearName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public TeacherDTO? Teacher { get; set; }
        public List<PupilDTO>? Pupils { get; set; }

    }

    public class CreateClubDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
    }
}
