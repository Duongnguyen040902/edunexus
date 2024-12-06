namespace sep.backend.v1.DTOs
{
    public class SchoolYearDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int SchoolId { get; set; }
    }

    public class CreateAndUpdateSchoolYearDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; } = false;
    }
}
