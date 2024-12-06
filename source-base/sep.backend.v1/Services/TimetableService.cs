using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using System.Linq.Expressions;

namespace sep.backend.v1.Services
{
    public class TimetableService : BaseService<TimeTable, TimeTableDTO>, ITimetableService
    {
        public TimetableService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {

        }

        public async Task<bool> CreateTimeTableAsync(TimeTableDTO model)
        {
            TimeTable timetable = _mapper.Map<TimeTableDTO, TimeTable>(model);
            var isExist = await _unitOfWork.GetRepository<TimeTable>()
                .GetSingleByCondition(x => x.ClassId == model.ClassId
            && x.SemesterId == model.SemesterId
            && x.DayOfWeek == model.DayOfWeek
            && x.TimeSlotId == model.TimeSlotId);
            if (isExist is not null)
            {
                throw new ConflictException("Thời khóa biểu đã tồn tại");
            }
            if (_unitOfWork.GetRepository<TimeTable>().Add(timetable).Result)
            {
                await _unitOfWork.CompleteAsync();

                return true;
            };

            return false;
        }

        public async Task<bool> DeleteTimeTableAsync(TimeTableDTO model)
        {
            Expression<Func<TimeTable, bool>> expression = x =>
                x.ClassId == model.ClassId
                && x.SemesterId == model.SemesterId
                && x.TimeSlotId == model.TimeSlotId
                && x.DayOfWeek == model.DayOfWeek;

            var records = await _unitOfWork.GetRepository<TimeTable>().GetMulti(expression);
            if (!records.Any())
            {
                throw new NotFoundException("Không tìm thấy thời khóa biểu");
            }
            _unitOfWork.GetRepository<TimeTable>().DeleteMulti(expression);

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateTimeTableAsync(TimeTableDTO model)
        {
            Expression<Func<TimeTable, bool>> expression = x =>
                x.ClassId == model.ClassId
                && x.SemesterId == model.SemesterId
                && x.TimeSlotId == model.TimeSlotId
                && x.DayOfWeek == model.DayOfWeek;

            var isExist = await _unitOfWork.GetRepository<TimeTable>().GetSingleByCondition(expression);
            if (isExist is null)
            {
                throw new NotFoundException("Không tim thấy thời khóa biểu");
            }
            var isdeleted = await DeleteTimeTableAsync(model);
            if (isdeleted)
            {
                TimeTable timetable = _mapper.Map<TimeTableDTO, TimeTable>(model);
                timetable.SubjectId = model.SubjectId;
                if (await _unitOfWork.GetRepository<TimeTable>().Add(timetable))
                {
                    await _unitOfWork.CompleteAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<List<TimeTableDetailDTO>> GetTimeTableDetailAsync(int classId, int semesterId)
        {
            var timetable = await _unitOfWork.GetRepository<TimeTable>()
                .GetMulti(x => x.ClassId == classId && x.SemesterId == semesterId,
                    new string[] { "Class", "Semester", "TimeSlot", "Subject" });
            if (timetable.Count() == 0)
            {
                return null;
            }

            return timetable.Select(t => _mapper.Map<TimeTable, TimeTableDetailDTO>(t)).ToList();

        }

    }
}
