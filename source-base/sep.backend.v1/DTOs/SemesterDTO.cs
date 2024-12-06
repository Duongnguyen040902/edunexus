using Microsoft.OpenApi.Models;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class SemesterDTO
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int SchoolYearId { get; set; }
    }

    public class ViewSemesterDTO
    {
        public int Id { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int SchoolYearId { get; set; }
        public string SchoolYearName { get; set; }
    }

    public class CreateAndUpdateSemesterDTO
    {
        public int? Id { get; set; }
        public string? SemesterName { get; set; }
        public string? SemesterCode { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; } = false;
        public int? SchoolYearId { get; set; }
    }
}
