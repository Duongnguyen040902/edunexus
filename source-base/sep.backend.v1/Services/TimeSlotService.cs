using sep.backend.v1.Common.Const;
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
    public class TimeSlotService : BaseService<TimeSlotDTO, TimeSlot>, ITimeSlotService
    {
        public TimeSlotService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<List<TimeSlotDTO>> GetAll(int schoolId)
        {
            var timeSlots = await _unitOfWork.GetRepository<TimeSlot>().GetMulti(x => x.SchoolId == schoolId);
            if (!timeSlots.Any())
            {
                return null;
            }

            var sortedTimeSlots = timeSlots.OrderBy(x => x.StartTime);
            return _mapper.Map<IEnumerable<TimeSlot>, List<TimeSlotDTO>>(sortedTimeSlots);
        }
     public async Task<PagedResponse<List<TimeSlotDTO>>> GetAllTimeSlots(
                    PaginationFilter filters,
                    IUriService uriService,
                    string route,
                    bool? isActive,
                    string? keyword,
                    int schoolId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);

            var pagedData = await _unitOfWork.TimeSlotRepository.GetAllTimeSlots(isActive, keyword, schoolId);
            var totalRecords = pagedData.Count();

            var timeSlotDtos = _mapper.Map<List<TimeSlot>, List<TimeSlotDTO>>((List<TimeSlot>)pagedData);

            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                timeSlotDtos.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }

        public async Task<TimeSlotDTO> GetTimeSlotDetail(int id)
        {
            var timeSlot = await _unitOfWork.TimeSlotRepository.GetById(id);
            if (timeSlot == null)
            {
                var placeholders = new Dictionary<string, string>
                        {
                            { "attribute", "Tiết học" }
                        };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            return _mapper.Map<TimeSlot, TimeSlotDTO>(timeSlot);
        }

        public async Task<TimeSlotDTO> CreateTimeSlot(int schoolId, CreateTimeSlotDTO timeSlotDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingTimeSlot = await _unitOfWork.TimeSlotRepository.GetSingleByCondition(
                    ts => ts.Name == timeSlotDto.Name && ts.SchoolId == schoolId);
                if (existingTimeSlot != null)
                {
                    var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "Tên tiết học" }
                            };
                    throw new ConflictException(StringHelper.FormatMessage(Responses.ConflictNameTemplate, placeholders));
                }
                var timeSlot = _mapper.Map<CreateTimeSlotDTO, TimeSlot>(timeSlotDto);
                timeSlot.SchoolId = schoolId;
                await _unitOfWork.TimeSlotRepository.Add(timeSlot);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<TimeSlot, TimeSlotDTO>(timeSlot);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TimeSlotDTO> UpdateTimeSlot(int id, CreateTimeSlotDTO timeSlotDto, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingTimeSlot = await _unitOfWork.TimeSlotRepository.GetById(id);
                if (existingTimeSlot == null)
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "attribute", "Tiết học" }
                    };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                var duplicateTimeSlot = await _unitOfWork.TimeSlotRepository.GetSingleByCondition(
                    ts => ts.Name == timeSlotDto.Name && ts.SchoolId == schoolId && ts.Id != id);
                if (duplicateTimeSlot != null)
                {
                    var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "Tên tiết học" }
                            };
                    throw new ConflictException(StringHelper.FormatMessage(Responses.ConflictNameTemplate, placeholders));
                }
                _mapper.Map(timeSlotDto, existingTimeSlot);
                _unitOfWork.TimeSlotRepository.Update(existingTimeSlot);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<TimeSlot, TimeSlotDTO>(existingTimeSlot);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<bool> DeleteTimeSlot(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var timeSlot = await _unitOfWork.TimeSlotRepository.GetById(id);
                if (timeSlot == null)
                {
                    var placeholders = new Dictionary<string, string>
                    {
                        { "attribute", "Tiết học" }
                    };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _unitOfWork.TimeSlotRepository.Delete(timeSlot.Id);
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
    }
}

