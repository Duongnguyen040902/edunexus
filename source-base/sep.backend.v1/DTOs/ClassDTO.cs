using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class ClassDTO
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int SchoolId { get; set; }
        public int Status { get; set; }
    }

    public class ClassDetailDTO
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int? SemesterId { get; set; }
        public string? SemesterName { get; set; }
        public int? SchoolYearId { get; set; }
        public string? SchoolYearName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int Status { get; set; }
        public TeacherDTO? HomeroomTeacher { get; set; }
        public List<PupilDTO>? Pupils { get; set; }
    }

public class ViewClassAdminDTO
    {
        public int Id { get; set; }
        public string ClassName { get; set; }
        public int SchoolId { get; set; }
        public int Status { get; set; }
        public string Block { get; set; }
    }

    public class AddClassDTO
    {
        public string Name { get; set; }

        public string Block { get; set; }
    }
      public class UpdateClassDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Block { get; set; }
    }

    public class ViewClassDetailDTO
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int? SemesterId { get; set; }
        public string? SemesterName { get; set; }
        public int? SchoolYearId { get; set; }
        public string? SchoolYearName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int Status { get; set; }
        public TeacherDTO? HomeroomTeacher { get; set; }
    }
}
