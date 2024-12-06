namespace sep.backend.v1.DTOs
{
    public class PupilScoreDTO
    {
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public float Score { get; set; }
        public int SemesterId { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class PupilScoreViewDTO
    {
        public int SubjectId { get; set; }
        public int PupilId { get; set; }
        public float? Score { get; set; }
        public int SemesterId { get; set; }
        public int Status { get; set; }
        public string? SubjectName { get; set; }
        public string? PupilName { get; set; }
        public string? Image { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class ClassScoreListDTO
    {
        public List<SubjectScoreDTO> Subjects { get; set; }
        public List<PupilScoreSummaryDTO> Pupils { get; set; }
    }

    public class SubjectScoreDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class PupilScoreSummaryDTO
    {
        public int PupilId { get; set; }
        public string? PupilName { get; set; }
        public string? Image { get; set; }
        public List<PupilSubjectScoreDTO> SubjectScores { get; set; }
    }

    public class PupilSubjectScoreDTO
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public List<float?>? Scores { get; set; }
    }

    public class PupilIndividualScoreDTO
    {
        public string SubjectName { get; set; }
        public List<float?>? Scores { get; set; }
    }
}