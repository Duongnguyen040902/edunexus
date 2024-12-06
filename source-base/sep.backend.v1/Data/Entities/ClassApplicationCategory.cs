namespace sep.backend.v1.Data.Entities
{
    public class ClassApplicationCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ClassApplication>? ClassApplications { get; set; }
    }
}
