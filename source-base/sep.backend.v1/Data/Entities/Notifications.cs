namespace sep.backend.v1.Data.Entities
{
    public class Notifications: BaseEntity
    { 
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Title { get; set; }
        public string Descriptions { get; set; }
        public int CategoryId { get; set; }
        public int SchoolYearId { get; set; }


        public virtual Class? Class { get; set; }
        public virtual SchoolYear? SchoolYear { get; set; }
        public virtual NotificationCategory? Category { get; set; }
        public virtual ICollection<NotificationImage>? Images { get; set; }
    }
}
