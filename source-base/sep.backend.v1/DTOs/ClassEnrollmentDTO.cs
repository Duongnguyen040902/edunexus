namespace sep.backend.v1.DTOs
{
    public class ClassEnrollmentDTO
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int SemesterId { get; set; }
    }

    public class AssignTeacherRequest
    {
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int SemesterId { get; set; }
    }

    public class UpdateAssignTeacherRequest
    {

        public int ClassEnrollmentId { get; set; }
        public int ClassId { get; set; }
        public int TeacherId { get; set; }
        public int SemesterId { get; set; }
    }
    public class AssignPupilRequest
    {
        public int PupilId { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
    }

    public class AssignMemberToClass
    {
        public int? PupilId { get; set; }
        public int? TeacherId { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
    }

    public class TeacherSwapDTO
    {
        public int ClassEnrollmentId { get; set; }
        public int SemesterId { get; set; }
        public string TeacherName { get; set; }
        public string ClassName { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
    }

    public class ViewClassEnrollDTO
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SchoolYearId { get; set; }
        public string SchoolYearName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
    }

    public class MemberInClassDTO
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int? Block { get; set; }
        public string? ClassName { get; set; }
        public string? TeacherId { get; set; }
        public string? TeacherName { get; set; } 
        public string? TeacherCode { get; set; }
        public string? TeacherImage { get; set; }
        public string? PupilName { get; set; }
        public string? PupilId { get; set; }
        public string? PupilCode { get; set; }
        public string? PupilImage { get; set; }
        public int SemesterId { get; set; }

    }

 }
