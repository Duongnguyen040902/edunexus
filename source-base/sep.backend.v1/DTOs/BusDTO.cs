using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class BusDTO
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

    public class ViewBusDetailDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? LicensePlate { get; set; }
        public int SeatNumber { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
        public string? BusRouteName { get; set; }
        public BusSupervisorDetailDTO? BusSupervisor { get; set; }
        public List<PupilOnBusDTO>? Pupil { get; set; }
        public List<ViewBusStopDTO>? BusStops { get; set; }
    }
    public class ViewBusStopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public string Address { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
    }
    public class CreateBusDto
    {
        public string Name { get; set; }
        public string? DriverName { get; set; }
        public string? DriverPhone { get; set; }
        public string? LicensePlate { get; set; }
        public int SeatNumber { get; set; }
        public int BusRouteId { get; set; }
        public int Status { get; set; }
    }

    public class BusDetailDto : BusDTO
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

    public class ViewBusEnrollDetailDTO
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public string BusName { get; set; }
        public int SemesterId { get; set; }
        public string SemesterName { get; set; }
        public int SchoolYearId { get; set; }
        public string SchoolYearName { get; set; }
        public int? BusStopId { get; set; }
        public string? BusStopName { get; set; }
        public TimeSpan? PickUpTime { get; set; }
        public TimeSpan? ReturnTime { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrent { get; set; }
    }
    
    public class PupilOnBusDTO
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public string? PupilName { get; set; }
        public string? Image { get; set; }
        public string? DonorName { get; set; }
        public string? DonorPhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Gender { get; set; }
        public int? BusStopId { get; set; }
        public string? BusStopAddress { get; set; }
    }

}
