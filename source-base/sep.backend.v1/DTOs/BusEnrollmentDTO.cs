using sep.backend.v1.Data.Entities;

namespace sep.backend.v1.DTOs
{
    public class CreateBusEnrollmentDTO
    {
        public int BusId { get; set; }
        public int? PupilId { get; set; }
        public int? BusSupervisorId { get; set; }
        public int SemesterId { get; set; }
        public int BusStopId { get; set; }
    }

    public class BusEnrollmentDTO
    {
        public int Id { get; set; }
        public int BusId { get; set; }
        public int? PupilId { get; set; }
        public int? BusSupervisorId { get; set; }
        public int SemesterId { get; set; }

        public string? PupilName { get; set; }        
        public string? PupilCode { get; set; }        
        public string? SemesterName { get; set; }      
        public string? AcademicYear { get; set; }    
        public string? BusName { get; set; }         
        public string? BusSupervisorName { get; set; }
        public string? BusSupervisorCode { get; set; }

        public int? BusStopId { get; set; }
        public string? BusStopName { get; set; }
    }
}