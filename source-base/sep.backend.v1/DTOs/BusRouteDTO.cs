namespace sep.backend.v1.DTOs
{
    public class BusRouteDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }

    }

    public class CreateBusRouteDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Status { get; set; }

    }

    public class BusRouteDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SchoolId { get; set; }
        public int Status { get; set; }

        public List<BusStopViewDTO> BusStops { get; set; } = new List<BusStopViewDTO>();
        public List<BusViewDTO> Buses { get; set; } = new List<BusViewDTO>();
    }
    public class BusStopViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan PickUpTime { get; set; }
        public TimeSpan ReturnTime { get; set; }
        public string Address { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
    }

    public class BusViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? LicensePlate { get; set; }
        public int SeatNumber { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
    }
}

