namespace sep.backend.v1.Data.Entities
{
    public class NotificationImage : BaseEntity
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int NotificationId { get; set; }

        public Notifications? Notification { get; set; }

    }
}
