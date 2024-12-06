namespace sep.backend.v1.DTOs
{
    public class AttendanceRecordViewDTO
    {
        public int Id { get; set; }
        public int PupilId { get; set; }
        public string? PupilName { get; set; }
        public string? Image { get; set; }
        public int? ClassId { get; set; }
        public int? ClubId { get; set; }
        public int? BusId { get; set; }
        public bool? IsAttend { get; set; }
        public int AttendanceSession { get; set; }
        public int AttendanceType { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AttendanceRecordDTO
    {
        public int? Id { get; set; }
        public int PupilId { get; set; }
        public int? ClassId { get; set; }
        public int? ClubId { get; set; }
        public int? BusId { get; set; }
        public bool? IsAttend { get; set; }
        public int AttendanceSession { get; set; }
        public int AttendanceType { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PupilAttendance
    {
        public PupilAttendanceMaterial PupilAttendanceMaterial { get; set; }
        public List<PupilViewAttendanceDTO> PupilViewAttendanceDTO { get; set; }
    }
    public class PupilViewAttendanceDTO
    {
        public DateTime CreateDate { get; set; }
        public int? Type { get; set; }
        public ViewClassAttendanceDTO? IsAttendClass { get; set; }
        public List<ViewClubAttendanceDTO>? IsAttendClub { get; set; }
        public ViewBusAttendanceDTO? IsAttendBus { get; set; }
    }

    public class ViewClubAttendanceDTO
    {
        public int? ClubId { get; set; }
        public bool? IsAttend { get; set; }
        public string? Feedback { get; set; }
    }

    public class ViewClassAttendanceDTO
    {
        public int? ClassId { get; set; }
        public bool? IsAttend { get; set; }
        public string? Feedback { get; set; }

    }

    public class ViewBusAttendanceDTO
    {
        public int? BusId { get; set; }
        public bool? IsAttend { get; set; }
        public string? Feedback { get; set; }
    }

    public class PupilAttendanceMaterial
    {
        public string ClassName { get; set; }
        public string[] ClubName { get; set; }
        public string BusName { get; set; }
    }
}