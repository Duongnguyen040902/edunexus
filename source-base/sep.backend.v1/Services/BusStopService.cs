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
    public class BusStopService : BaseService<BusStopDTO, BusStop>, IBusStopService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapper _mapper;

        public BusStopService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BusStopDTO> CreateBusStop(CreateBusStopDTO busStopDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingBusStops = await _unitOfWork.BusStopRepository
                    .GetMulti(busStop => busStop.BusRouteId == busStopDto.BusRouteId);
                var duplicateBusStop = existingBusStops.FirstOrDefault(bs => bs.Name == busStopDto.Name);
                if (duplicateBusStop != null)
                {
                    throw new ConflictException(Responses.DuplicateBusStopName);
                }
                int nextIndex = existingBusStops.Any() ? existingBusStops.Max(bs => bs.Index) + 1 : 1;
                if (busStopDto.ReturnTime <= busStopDto.PickUpTime)
                {
                    throw new ConflictException(Responses.InvalidTime);
                }
                var latestBusStop = existingBusStops.OrderByDescending(bs => bs.PickUpTime).FirstOrDefault();
                if (latestBusStop != null)
                {
                    if (busStopDto.PickUpTime <= latestBusStop.PickUpTime)
                    {
                        throw new ConflictException(Responses.InvalidPickUpTime);
                    }
                    if (busStopDto.ReturnTime < latestBusStop.ReturnTime)
                    {
                        throw new ConflictException(Responses.InvalidReturnTime);
                    }
                }
                var newBusStop = _mapper.Map<CreateBusStopDTO, BusStop>(busStopDto);
                newBusStop.Index = nextIndex;

                await _unitOfWork.BusStopRepository.Add(newBusStop);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<BusStop, BusStopDTO>(newBusStop);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<BusStopDTO?> UpdateBusStop(int id, CreateBusStopDTO busStopDto)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busStop = await _unitOfWork.BusStopRepository.GetById(id);
                if (busStop == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "điểm dừng" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                var duplicateBusStop = await _unitOfWork.BusStopRepository
                    .GetSingleByCondition(bs => bs.BusRouteId == busStop.BusRouteId && bs.Name == busStopDto.Name && bs.Id != id);
                if (duplicateBusStop != null)
                {
                    throw new ConflictException(Responses.DuplicateBusStopName);
                }
                var previousBusStop = await _unitOfWork.BusStopRepository.GetSingleByCondition(bs => bs.Index == (busStop.Index - 1));
                if (previousBusStop != null)
                {
                    if (busStopDto.PickUpTime <= previousBusStop.PickUpTime)
                    {
                        throw new ConflictException(Responses.InvalidUpdatePickUpTime1);
                    }
                    if (busStopDto.ReturnTime <= previousBusStop.ReturnTime)
                    {
                        throw new ConflictException(Responses.InvalidUpdateReturnTime1);
                    }
                }
                var nextBusStop = await _unitOfWork.BusStopRepository.GetSingleByCondition(bs => bs.Index == (busStop.Index + 1));
                if (nextBusStop != null)
                {
                    if (busStopDto.PickUpTime >= nextBusStop.PickUpTime)
                    {
                        throw new ConflictException(Responses.InvalidUpdatePickUpTime2);
                    }
                    if (busStopDto.ReturnTime >= nextBusStop.ReturnTime)
                    {
                        throw new ConflictException(Responses.InvalidUpdateReturnTime2);
                    }
                }
                _mapper.Map(busStopDto, busStop);
                _unitOfWork.BusStopRepository.Update(busStop);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<BusStop, BusStopDTO>(busStop);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<bool> DeleteBusStop(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busStop = await _unitOfWork.BusStopRepository.GetById(id);
                if (busStop == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "điểm dừng" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }

                _unitOfWork.BusStopRepository.Delete(busStop.Id);
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

        public async Task<BusStopDTO?> GetBusStopDetail(int id)
        {
            var busStops = await _unitOfWork.BusStopRepository.GetMulti(
                x => x.Id == id,
                new string[] { "Route", "Registrations" }
            );
            var busStop = busStops.FirstOrDefault();
            if (busStop is null)
            {
                var placeholders = new Dictionary<string, string> { { "attribute", "điểm dừng" } };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            return _mapper.Map<BusStop, BusStopDTO>(busStop);
        }

        public async Task<PagedResponse<List<BusStopDTO>>> GetListBusStop(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int busRouteId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var busStops = await _unitOfWork.BusStopRepository.GetBusStops(status, keyword, busRouteId);
            var totalRecords = busStops.Count();
            var busStopDtoList = _mapper.Map<List<BusStop>, List<BusStopDTO>>(busStops);

            return PagedResponseHelper.CreatePagedResponse(
                busStopDtoList.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );
        }
    }
}
