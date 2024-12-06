using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class ClubEnrollmentDTO
    {
        public int? ClubId { get; set; }
        public int? Status { get; set; }
    }

    public class AssignMemberRequest
    {
        public int ClubId { get; set; }
        public int? PupilId { get; set; }
        public int? TeacherId { get; set; }
        public int Status { get; set; }
        public int SemesterId { get; set; }
    }

    public class UpdateMemberRequest
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int? PupilId { get; set; }
        public int? TeacherId { get; set; }
        public int Status { get; set; }
        public int SemesterId { get; set; }
    }
    public class ClubEnrollmentDetailDTO
    {
        public int ClubId { get; set; }
        public int? PupilId { get; set; }
        public int Status { get; set; }
        public int SemesterId { get; set; }
        public string ClubName { get; set; }
        public string ClubDescription { get; set; }
        public string SemesterName { get; set; }
        public TeacherDTO? Teacher{ get; set; }
    }

    public class ClubEnrollmentForAdminSchoolDTO
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int? PupilId { get; set; }
        public string? PupilName { get; set; }
        public string? PupilUsername { get; set; }
        public int? TeacherId { get; set; }
        public string? TeacherName { get; set; }
        public string? TeacherUsername { get; set; }
        public int Status { get; set; }
        public int SemesterId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
