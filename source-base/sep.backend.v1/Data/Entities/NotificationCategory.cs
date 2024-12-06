namespace sep.backend.v1.Data.Entities
{
    public class NotificationCategory 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Notifications>? Notifications { get; set; }

    }
}