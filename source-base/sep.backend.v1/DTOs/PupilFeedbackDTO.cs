namespace sep.backend.v1.DTOs
{
    public class PupilFeedbackDTO
    {
        public int PupilId { get; set; }
        public int SemesterId { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class ListPupilFeedbackDTO
    {
        public int PupilId { get; set; }
        public string? PupilName { get; set; }
        public string? DonorName { get; set; }
        public string? Image { get; set; }
        public int SemesterId { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class RequestGetPupilFeedbackDTO
    {
        public int PupilId { get; set; }
        public int SemesterId { get; set; }
    }

    public class PupilFeedbackDetailDTO
    {
        public ViewSemesterDTO Semester { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}