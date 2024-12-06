namespace sep.backend.v1.Data.Entities
{
    public class Bus : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? LicensePlate { get; set; }
        public int SeatNumber { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
        public virtual BusRoute? BusRoute { get; set; }
        public virtual ICollection<AttendanceRecord>? AttendanceRecords { get; set; }
        public virtual ICollection<BusEnrollment>? BusEnrollments { get; set; }
        
    }
}
