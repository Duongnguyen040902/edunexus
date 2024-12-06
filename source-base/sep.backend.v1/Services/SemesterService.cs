using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class SemesterService : BaseService<SemesterDTO, Semester>, ISemesterService
    {
        public SemesterService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }
        
        public async Task<IEnumerable<ViewSemesterDTO>> GetAllAsync(int schoolId)
        {
            var schoolYears = await _unitOfWork.GetRepository<SchoolYear>().GetMulti(x => x.SchoolId == schoolId, new string[] { "Semesters", "School" });
            var data = schoolYears.SelectMany(x => x.Semesters).OrderByDescending(x => x.StartDate).ToList();
            if (data.Any())
            {
                return data.Select(x => _mapper.Map<Semester, ViewSemesterDTO>(x));
            }
            return null;
        }

        public Task<SemesterDTO> GetNewSemester(int schoolId)
        {
            var semester = _unitOfWork.GetRepository<Semester>().GetMulti(x => x.IsActive && x.SchoolYear.Id == schoolId).Result.OrderByDescending(x => x.Id).FirstOrDefault();
            if (semester is null)
            {
                throw new NotFoundException("Not found");
            }

            return Task.FromResult(_mapper.Map<Semester, SemesterDTO>(semester));
        }

        public async Task<SemesterDTO> GetCurrentSemester(int schoolId)
        {
            var semester = await _unitOfWork.GetRepository<Semester>()
                .GetSingleByCondition(x => x.IsActive && x.SchoolYear.SchoolId == schoolId
                    , new string[] { "SchoolYear" });
            if (semester is null)
            {
                throw new NotFoundException("Không có học kỳ nào đang hoạt động");
            }

            return _mapper.Map<Semester, SemesterDTO>(semester);
        }
        public async Task<List<ViewSemesterDTO>> GetListSemesters(int schoolId, int schoolYearId)
        {
            var semesters = await _unitOfWork.GetRepository<SchoolYear>()
                .GetMulti(x => x.SchoolId == schoolId && x.Id == schoolYearId, new string[] { "Semesters", "School" });
            var data = semesters.SelectMany(x => x.Semesters).OrderByDescending(x => x.StartDate).ToList();
            if (data.Count() == 0)
            {
                return null;
            }
            var semesterDTOs = _mapper.Map<List<Semester>, List<ViewSemesterDTO>>(data);

            return semesterDTOs;
        }
        public async Task<SemesterDTO> GetSemester(int Id)
        {
            var semester = await _unitOfWork.GetRepository<Semester>().GetSingleByCondition(x => x.Id == Id);
            if (semester is null)
            {
                throw new NotFoundException("Không tìm thấy kỳ học");
            }
            var semesterDTO = _mapper.Map<Semester, SemesterDTO>(semester);

            return semesterDTO;
        }
        public async Task<bool> Create(CreateAndUpdateSemesterDTO semesterDTO)
        {
            var semester = _mapper.Map<CreateAndUpdateSemesterDTO, Semester>(semesterDTO);
            await _unitOfWork.GetRepository<Semester>().Add(semester);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<bool> Update(CreateAndUpdateSemesterDTO semesterDTO, int schoolId)
        {
            var endDate = await _unitOfWork.GetRepository<Semester>().Where(x => x.Id == semesterDTO.Id).Select(x => x.EndDate).FirstOrDefaultAsync();
            if (endDate < DateTime.Now)
            {
                throw new ConflictException("Không thể update kỳ học đã kết thúc");
            }
            var semester = _mapper.Map<CreateAndUpdateSemesterDTO, Semester>(semesterDTO);
            if(semester.IsActive && semester.StartDate <= DateTime.Now && DateTime.Now <= semester.EndDate)
            {
                // Tìm năm học có trạng thái active
                var schoolYearActive = await _unitOfWork.GetRepository<SchoolYear>().GetSingleByCondition(x => x.IsActive == true);
                // Tìm năm học của kì này
                var schoolYear = await _unitOfWork.GetRepository<SchoolYear>().GetSingleByCondition(x => x.Id == semester.SchoolYearId);
                //Tắt năm học đang active, active năm học tiện tại
                if (schoolYearActive is null)
                {
                    schoolYear.IsActive = true;
                    await _unitOfWork.GetRepository<SchoolYear>().Update(schoolYear);
                    await _unitOfWork.CompleteAsync();
                }else if(schoolYear.Id != schoolYearActive.Id)
                {
                    schoolYearActive.IsActive = false;
                    await _unitOfWork.GetRepository<SchoolYear>().Update(schoolYearActive);
                    schoolYear.IsActive = true;
                    await _unitOfWork.GetRepository<SchoolYear>().Update(schoolYear);
                    await _unitOfWork.CompleteAsync();
                }

                var activeSemester = await _unitOfWork.GetRepository<Semester>().GetSingleByCondition(x => x.IsActive && x.SchoolYear.SchoolId == schoolId&&x.Id!=semester.Id);
                if (activeSemester is not null)
                {
                    activeSemester.IsActive = false;
                    await _unitOfWork.GetRepository<Semester>().Update(activeSemester);
                    await _unitOfWork.CompleteAsync();
                }
            }
            else if (semester.IsActive && semester.StartDate > DateTime.Now)
            {
                throw new ConflictException("Kỳ này chưa thể bật hoạt động");
            }
            semester.UpdatedDate = DateTime.Now;
            await _unitOfWork.GetRepository<Semester>().Update(semester);
            await _unitOfWork.CompleteAsync();

            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var semesterExit = await _unitOfWork.GetRepository<Semester>().GetSingleByCondition(x => x.Id == id);
            if (semesterExit.IsActive)
            {
                throw new ConflictException("Không thể xóa kỳ học đang hoạt động");
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var findSemester = await _unitOfWork.GetRepository<Semester>().GetSingleByCondition(x => x.Id == id);
                if (findSemester is null)
                {
                    throw new NotFoundException("Không tìm thấy kỳ học");
                }
                await _unitOfWork.GetRepository<Semester>().Delete(id);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new ConflictException("Đã tồn tại dữ liệu liên kết");
            }
        }
        public async Task<SemesterDTO> GetNextSemester(int schoolId)
        {
            var currentSemester = await _unitOfWork.GetRepository<Semester>()
                .GetSingleByCondition(x => x.IsActive && x.SchoolYear.SchoolId == schoolId, new string[] { "SchoolYear" });

            if (currentSemester is null)
            {
                throw new NotFoundException("Không có học kỳ nào đang hoạt động");
            }

            var nextSemester = await _unitOfWork.GetRepository<Semester>()
                .Where(x => x.SchoolYear.SchoolId == schoolId && x.StartDate > currentSemester.StartDate)
                .OrderBy(x => x.StartDate)
                .FirstOrDefaultAsync();

            if (nextSemester is null)
            {
                throw new NotFoundException("Không có học kỳ tiếp theo");
            }

            return _mapper.Map<Semester, SemesterDTO>(nextSemester);
        }

    }
}
