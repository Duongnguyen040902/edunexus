using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using System.Linq.Expressions;

namespace sep.backend.v1.Services
{
    public class BusService : BaseService<BusDTO, Bus>, IBusService
    {
        public BusService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<BusDTO> GetBusByConditionAsync(Expression<Func<BusEnrollment, bool>> predicate)
        {
            var busEnrollment = await _unitOfWork.GetRepository<BusEnrollment>()
                .GetSingleByCondition(predicate, new string[] { "Semester", "Bus" });

            if (busEnrollment == null)
            {
                return null;
            }
            var bus = busEnrollment.Bus;

            return _mapper.Map<Bus, BusDTO>(bus);
        }

        public async Task<BusDTO> GetAssignedBus(int supervisorId)
        {
            return await GetBusByConditionAsync(x => x.Semester.IsActive && x.BusSupervisorId == supervisorId);
        }

        public async Task<ViewBusDetailDTO> GetBusDetail(int busId, int semesterId)
        {
            var bus = await _unitOfWork.BusRepository.GetBusDetailAsync(busId, semesterId);
            if (bus == null)
            {
                throw new NotFoundException("Không tìm thấy xe trong kỳ học này");
            }
            var result = _mapper.Map<Bus, ViewBusDetailDTO>(bus);

            return result;
        }

        public async Task<BusDTO> GetEnrollmentBus(int pupilId)
        {
            return await GetBusByConditionAsync(x => x.Semester.IsActive && x.PupilId == pupilId);
        }
         public async Task<BusDTO> CreateBus(CreateBusDto busDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingBus = await _unitOfWork.BusRepository.GetSingleByCondition(b => b.Name == busDto.Name);
                if (existingBus != null)
                {
                    throw new ConflictException(Responses.ConflictBusName);
                }
                var newBus = _mapper.Map<CreateBusDto, Bus>(busDto);
                await _unitOfWork.BusRepository.Add(newBus);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<Bus, BusDTO>(newBus);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<BusDTO?> UpdateBus(int id, CreateBusDto busDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var bus = await _unitOfWork.BusRepository.GetById(id);
                if (bus == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "xe" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                var existingBus = await _unitOfWork.BusRepository.GetSingleByCondition(b => b.Name == busDto.Name && b.Id != id);
                if (existingBus != null)
                {
                    throw new ConflictException(Responses.ConflictBusName);
                }
                _mapper.Map(busDto, bus);
                _unitOfWork.BusRepository.Update(bus);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<Bus, BusDTO>(bus);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteBus(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var bus = await _unitOfWork.BusRepository.GetById(id);
                if (bus == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "xe" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _unitOfWork.BusRepository.Delete(bus.Id);
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

        public async Task<BusDetailDto?> GetBusDetail(int id)
        {
            var buses = await _unitOfWork.BusRepository.GetMulti(
                x => x.Id == id,
                new string[] { "BusRoute", "AttendanceRecords", "BusEnrollments" }
            );

            var bus = buses.FirstOrDefault();
            if (bus == null)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "xe" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            return _mapper.Map<Bus, BusDetailDto>(bus);
        }

        public async Task<PagedResponse<List<BusDTO>>> GetListBus(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int schoolId, int busRouteId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.BusRepository.GetBuses(status, keyword, schoolId, busRouteId);
            var totalRecords = pagedData.Count();
            var busDto = _mapper.Map<List<Bus>, List<BusDTO>>(pagedData);

            return PagedResponseHelper.CreatePagedResponse(
                busDto.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );
        }

        public async Task<List<ViewBusEnrollDetailDTO>> GetViewBusEnrolls(int pupilId)
        {
            var busEnrollments = (await _unitOfWork.GetRepository<BusEnrollment>()
                        .GetMulti(ce => ce.PupilId == pupilId, new string[] { "Bus", "Semester.SchoolYear", "BusStop" }))
                        .OrderByDescending(x => x.Semester.EndDate);

            if (busEnrollments is null || !busEnrollments.Any())
            {
                return null;
            }

            return _mapper.Map<List<BusEnrollment>, List<ViewBusEnrollDetailDTO>>(busEnrollments.ToList());
        }
    }
}
