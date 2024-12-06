namespace sep.backend.v1.DTOs
{
    public class CreateBusStopDTO
    {
        public string? Name { get; set; }
        public TimeSpan? PickUpTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }
        public string? Address { get; set; }
        public int? BusRouteId { get; set; }
        public int? Status { get; set; }
        public int? Index { get; set; }
    }

    public class BusStopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan PickUpTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Address { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
        public int Index { get; set; }
    }
}
