using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices
{
    public interface IAttendanceRecordService : IBaseService<AttendanceRecordDTO, AttendanceRecord>
    {
        Task<List<AttendanceRecordViewDTO>> GetPupilForAttendance(int entityId, int session, int type, int semesterId);
        Task<bool> CreateAttendanceRecords(List<AttendanceRecordDTO> attendanceRecords);
        Task<List<AttendanceRecordViewDTO>> GetAttendanceRecords(int entityId, int session, int type, int semesterId, DateTime date);
        Task<bool> UpdateAttendanceRecords(List<AttendanceRecordDTO> updatedRecords);
        Task<List<AttendanceListDTO>> GetAttendanceListDTOs(int entityId, int session, int type, int semesterId, DateTime date);
        Task<PupilAttendance> GetPupilAttedanceList(int pupilId, int semesterId, DateTime date);
    }
}
