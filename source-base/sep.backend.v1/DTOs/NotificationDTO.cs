namespace sep.backend.v1.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public int CategoryId { get; set; }
        public int SchoolYearId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class DetailNotificationDTO
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public int CategoryId { get; set; }
        public List<NotificationImageDTO>? notificationImages { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class NotificationImageDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int NotificationId { get; set; }
    }

    public class AddNotificationDTO
    {
        public int ClassId { get; set; }
        public string? Title { get; set; }
        public string? Descriptions { get; set; }
        public int? CategoryId { get; set; }
        public List<IFormFile>? FileImage { get; set; }
    }

    public class UpdateNotificationDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Descriptions { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? FileImage { get; set; }
    }

}