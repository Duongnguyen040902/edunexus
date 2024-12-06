namespace sep.backend.v1.Data.Entities
{
    public class BusStop : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan PickUpTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Address { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
        public int Index { get; set; }
        public virtual BusRoute? Route { get; set; }
        public virtual ICollection<BusRouteRegistration>? Registrations { get; set; }
        public virtual ICollection<BusEnrollment>? BusEnrollments { get; set; }
    }
}
