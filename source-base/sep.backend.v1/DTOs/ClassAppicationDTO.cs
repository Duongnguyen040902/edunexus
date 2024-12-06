namespace sep.backend.v1.DTOs
{
    public class ClassAppicationDTO
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public int SemesterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ApplicationCategoryId { get; set; }
        public string? Response { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class GetClassAppicationDetailDTO
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? DonorName { get; set; }
        public int SemesterId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ApplicationCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Response { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class CreateAndUpdateClassApplicationDTO
    {
        public int? Id { get; set; }
        public int PupilId { get; set; }
        public int? SemesterId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? ApplicationCategoryId { get; set; }

    }

    public class ResponeClassApplicationDTO
    {
        public int Id { get; set; }
        public string Response { get; set; }
        public int Status { get; set; }
    }

    public class ClassApplicationCategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
