namespace sep.backend.v1.Data.Entities
{
    public class ActivityLog : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public User User { get; set; }
    }
}
