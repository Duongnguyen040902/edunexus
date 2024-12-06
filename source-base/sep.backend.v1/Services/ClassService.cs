using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.Repositories;
using sep.backend.v1.Services.UnitOfWork;
using SQLitePCL;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;


namespace sep.backend.v1.Services
{
    public class ClassService : BaseService<ClassDTO, Class>, IClassService
    {
        public ClassService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<ClassDTO> GetAssignClassAsync(int teacherId)
        {
            var classes = await _unitOfWork.GetRepository<ClassEnrollment>()
                .GetMulti(
                    ce => ce.TeacherId == teacherId && ce.Semester.IsActive,
                    new string[] { "Class", "Semester" }
                );
            var result = classes
                .OrderByDescending(x => x.Semester.Id)
                .FirstOrDefault()?.Class;
            if (result is null)
            {
                return null;
            }
            var classDto = _mapper.Map<Class, ClassDTO>(result);

            return classDto;
        }

        public async Task<ClassDetailDTO> GetClassAsync(int classId, int semesterId)
        {

            var classes = await _unitOfWork.ClassRepository.GetClassDetail(classId, semesterId);
            if (classes is null)
            {
                return null;
            }
            var classDto = _mapper.Map<Class, ClassDetailDTO>(classes);

            return classDto;
        }

        public async Task<TeacherAccountDTO> GetCurrentTeacherOfClassAsync(int classId, int semesterId)
        {
            var classEnrollment = await _unitOfWork.GetRepository<ClassEnrollment>()
                .GetSingleByCondition(
                    ce => ce.ClassId == classId && ce.SemesterId == semesterId,
                    new string[] { "Teacher" }
                );

            if (classEnrollment?.Teacher != null)
            {

                return _mapper.Map<Teacher, TeacherAccountDTO>(classEnrollment.Teacher);
            }

            return null;
        }

        public Task<ClassDTO> UpdateClassAsync(ClassDTO classEntity)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteClass(int id)
        {
            var existingClass = await _unitOfWork.GetRepository<Class>().GetById(id);
            if (existingClass == null)
            {
                throw new NotFoundException(Responses.NotFoundClass);
            }     
            await _unitOfWork.BeginTransactionAsync();
            try
            {       
                _unitOfWork.GetRepository<Class>().Delete(existingClass.Id);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();            
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> UpdateClass(UpdateClassDTO updateClassDTO, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                updateClassDTO.Name = $"{updateClassDTO.Block}{updateClassDTO.Name}";
                var existingClass = await _unitOfWork.GetRepository<Class>().GetById(updateClassDTO.Id);
                if (existingClass is null)
                {
                    throw new NotFoundException(Responses.NotFoundClass);
                }
                var checkClass = await _unitOfWork.ClassRepository.GetSingleByCondition(x => x.Id != updateClassDTO.Id && x.Name == updateClassDTO.Name);
                if (checkClass != null)
                {
                    throw new ConflictException(Responses.ConflictAddNewClass);
                }
                _mapper.Map(updateClassDTO, existingClass);
                existingClass.UpdatedDate = existingClass.CreatedDate;
                var result = await _unitOfWork.GetRepository<Class>().Update(existingClass);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }    
        }

        public async Task<bool> AddNewClass(AddClassDTO addClassDTO, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                addClassDTO.Name = $"{addClassDTO.Block}{addClassDTO.Name}";
                var existingClass = await _unitOfWork.ClassRepository.GetSingleByCondition(c => c.Name == addClassDTO.Name && c.SchoolId == schoolId);
                if (existingClass != null)
                    throw new ConflictException(Responses.ConflictAddNewClass);
                var newClass = _mapper.Map<AddClassDTO, Class>(addClassDTO);
                newClass.SchoolId = schoolId;
                newClass.Status = 0;
                var result = await _unitOfWork.GetRepository<Class>().Add(newClass);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }     
        }

        public async Task<PagedResponse<List<SchoolDTO>>> GetAllAccountSchoolAdmin(PaginationFilter filters,
     IUriService uriService, string route, int? status, string? keyword, int? subscriptionPlanId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.SchoolRepository.getAllAccountSchoolAdmin(status, keyword, subscriptionPlanId);
            var totalRecords = pagedData.Count();
            var schoolsDto = _mapper.Map<List<School>, List<SchoolDTO>>(pagedData);

            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                schoolsDto.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }
        public async Task<PagedResponse<List<ViewClassAdminDTO>>> GetAllClass(
                        PaginationFilter filters,
                        IUriService uriService,
                        string route,
                        int schoolId,
                        string? keyword,
                        int? status)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.ClassRepository.getAllClass(schoolId, keyword, status);
            var totalRecords = pagedData.Count();
            var classesDto = _mapper.Map<List<Class>, List<ViewClassAdminDTO>>(pagedData);
            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                classesDto.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }

        public async Task<ClassDTO> GetEnrollmentClassAsync(int pupilId)
        {
            var classes = await _unitOfWork.GetRepository<ClassEnrollment>()
                .GetMulti(
                    ce => ce.PupilId == pupilId && ce.Semester.IsActive,
                    new string[] { "Class", "Semester" }
                );
            var result = classes
                .OrderByDescending(x => x.Semester.Id)
                .FirstOrDefault()?.Class;
            if (result is null)
            {
                return null;
            }
            var classDto = _mapper.Map<Class, ClassDTO>(result);

            return classDto;
        }

        public async Task<List<ViewClassEnrollDTO>> GetClassesByPupilIdAsync(int pupilId)
        {

            var classEnrollments = (await _unitOfWork.GetRepository<ClassEnrollment>()
            .GetMulti(ce => ce.PupilId == pupilId, new string[] { "Class", "Semester.SchoolYear" }))
            .OrderByDescending(x => x.Semester.EndDate);
            if (classEnrollments is null || !classEnrollments.Any())
            {
                throw new NotFoundException(Responses.NotFoundClass);
            }

            var result = _mapper.Map<List<ClassEnrollment>, List<ViewClassEnrollDTO>>(classEnrollments.ToList());

            return result;
        }

        public async Task<ViewClassAdminDTO> GetClassDetailByID(int classId)
        {
            var classes = await _unitOfWork.ClassRepository.GetSingleByCondition(x => x.Id == classId);
            if (classes is null)
            {
                throw new NotFoundException(Responses.NotFoundClass);
            }
            var classDto = _mapper.Map<Class, ViewClassAdminDTO>(classes);

            return classDto;
        }
    }
}
