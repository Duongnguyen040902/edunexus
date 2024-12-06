using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services
{
    public class SchoolYearService : BaseService<SchoolYearDTO, SchoolYear>, ISchoolYearService
    {
        public SchoolYearService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<SchoolYearDTO> GetSchoolYear(int schoolYearId)
        {
            var schoolYear = await _unitOfWork.GetRepository<SchoolYear>().Where(x => x.Id == schoolYearId).FirstOrDefaultAsync();
            if(schoolYear is null)
            {
                throw new NotFoundException("Không tìm thấy năm học");
            }

            return _mapper.Map<SchoolYear,SchoolYearDTO>(schoolYear);
        }

        public async Task<PagedResponse<List<SchoolYearDTO>>> GetSchoolYearsBySchoolId(PaginationFilter paginationFilter, IUriService uriService, string route, int schoolId)
        {
            var filter = new PaginationFilter(paginationFilter.PageNumber, paginationFilter.PageSize);
            var schoolYears = _unitOfWork.GetRepository<SchoolYear>().Where(x => x.SchoolId == schoolId).OrderByDescending(x => x.Id).ToList();
            
            var schoolYearsDTO = _mapper.Map<List<SchoolYear>, List<SchoolYearDTO>>(schoolYears);
            var totalRecords = schoolYears.Count;
            var pageResponse = PagedResponseHelper.CreatePagedResponse(schoolYearsDTO.AsQueryable(), filter, totalRecords, uriService, route);

            return pageResponse;
        }

        public async Task<bool> CreateSchoolYear(CreateAndUpdateSchoolYearDTO createSchoolYearDTO, int schoolId)
        {
            var schoolYear = _mapper.Map<CreateAndUpdateSchoolYearDTO, SchoolYear>(createSchoolYearDTO);
            schoolYear.SchoolId = schoolId;
            schoolYear.Name = schoolYear.StartDate.Year + "-" + schoolYear.EndDate.Year;
            await _unitOfWork.GetRepository<SchoolYear>().Add(schoolYear);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> UpdateSchoolYear(CreateAndUpdateSchoolYearDTO updateSchoolYearDTO, int schoolId)
        {
            var endDate = await _unitOfWork.GetRepository<SchoolYear>().Where(x => x.Id == updateSchoolYearDTO.Id).Select(x => x.EndDate).FirstOrDefaultAsync();
            if (endDate < DateTime.Now)
            {
                throw new ConflictException("Không thể update năm học đã kết thúc");
            }

            //off the current active school year
            if (updateSchoolYearDTO.IsActive.HasValue && updateSchoolYearDTO.IsActive.Value)
            {
                // Tương tác kì học
                var semester = await _unitOfWork.GetRepository<Semester>().Where(x => x.SchoolYearId == updateSchoolYearDTO.Id).OrderBy(x=>x.StartDate).FirstOrDefaultAsync();
                
               
                if (semester != null && semester.StartDate <= DateTime.Now && DateTime.Now <= semester.EndDate)
                {
                    //tìm kì học là true để chuyển thành false
                    var currentSemester = await _unitOfWork.GetRepository<Semester>().Where(x => x.IsActive == true).FirstOrDefaultAsync();
                    if (currentSemester != null)
                    {
                        currentSemester.IsActive = false;
                        _unitOfWork.GetRepository<Semester>().Update(currentSemester);
                        await _unitOfWork.CompleteAsync();
                    }
                    // bật kì học có start date nhỏ nhất của năm học này
                    semester.IsActive = true;
                    _unitOfWork.GetRepository<Semester>().Update(semester);
                }
                else
                {
                    throw new NotFoundException("Không tìm thấy học kì phù hợp trong năm học này");
                }

                var activeSchoolYear = await _unitOfWork.GetRepository<SchoolYear>()
                    .Where(sy => sy.IsActive == true && sy.SchoolId == schoolId && sy.Id!= updateSchoolYearDTO.Id).FirstOrDefaultAsync();

                if (activeSchoolYear != null)
                {
                    activeSchoolYear.IsActive = false;
                    _unitOfWork.GetRepository<SchoolYear>().Update(activeSchoolYear);
                }
            }

            var schoolYear = _mapper.Map<CreateAndUpdateSchoolYearDTO, SchoolYear>(updateSchoolYearDTO);
            schoolYear.UpdatedDate = DateTime.Now;
            schoolYear.SchoolId = schoolId;
            schoolYear.Name = schoolYear.StartDate.Year + "-" + schoolYear.EndDate.Year;
            await _unitOfWork.GetRepository<SchoolYear>().Update(schoolYear);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteSchoolYear(int id)
        {
            var schoolYearExist = await _unitOfWork.GetRepository<SchoolYear>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (schoolYearExist.IsActive)
            {
                throw new ConflictException("Không thể xóa năm học đang hoạt động");
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var findSchoolYear = await _unitOfWork.GetRepository<SchoolYear>().GetSingleByCondition(x => x.Id == id);
                if (findSchoolYear is null)
                {
                    throw new NotFoundException("Không có dữ liệu");
                }
                await _unitOfWork.GetRepository<SchoolYear>().Delete(id);
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

        
    }
}
