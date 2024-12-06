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
    public class BusRouteService : BaseService<BusRouteDTO, BusRoute>, IBusRouteService
    {
        public BusRouteService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
        {
        }

        public async Task<BusRouteDTO> CreateBusRoute(CreateBusRouteDto busRouteDto, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var existingBusRoute = await _unitOfWork.BusRouteRepository
                .GetSingleByCondition(br => br.Name == busRouteDto.Name && br.SchoolId == schoolId);
                if (existingBusRoute != null)
                {
                    throw new ConflictException(Responses.ConflictNameRoute);
                }
                var newBusRoute = _mapper.Map<CreateBusRouteDto, BusRoute>(busRouteDto);
                newBusRoute.SchoolId = schoolId;
                await _unitOfWork.BusRouteRepository.Add(newBusRoute);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<BusRoute, BusRouteDTO>(newBusRoute);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DeleteBusRoute(int id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busRoute = await _unitOfWork.BusRouteRepository.GetById(id);
                if (busRoute == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "tuyến xe" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                _unitOfWork.BusRouteRepository.Delete(busRoute.Id);
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

        public async Task<List<BusRouteDetailDTO>> GetBusRoute(int id)
        {
            var busRoute = await _unitOfWork.BusRouteRepository.GetMulti(x => x.Id == id, new string[] { "BusStops", "Buses" });
            if (busRoute is null)
            {
                var placeholders = new Dictionary<string, string>
            {
                { "attribute", "tuyến xe" }
            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }

            return _mapper.Map<List<BusRoute>, List<BusRouteDetailDTO>>((List<BusRoute>)busRoute);
        }

        public async Task<PagedResponse<List<BusRouteDTO>>> GetListBusRoute(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int? schoolId)
        {
            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var pagedData = await _unitOfWork.BusRouteRepository.getAllBusRoute(status, keyword, schoolId);
            var totalRecords = pagedData.Count();
            var busRouteDto = _mapper.Map<List<BusRoute>, List<BusRouteDTO>>(pagedData);

            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                busRouteDto.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
            );

            return pagedResponse;
        }

        public async Task<BusRouteDTO?> UpdateBusRoute(int id, CreateBusRouteDto busRouteDto, int schoolId)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var busRoute = await _unitOfWork.BusRouteRepository.GetById(id);
                if (busRoute == null)
                {
                    var placeholders = new Dictionary<string, string> { { "attribute", "tuyến xe" } };
                    throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
                }
                var existingBusRoute = await _unitOfWork.BusRouteRepository
                .GetSingleByCondition(br => br.Name == busRouteDto.Name && br.Id != id && br.SchoolId == schoolId);
                if (existingBusRoute != null)
                {
                    throw new ConflictException(Responses.ConflictNameRoute);
                }
                _mapper.Map(busRouteDto, busRoute);
                _unitOfWork.BusRouteRepository.Update(busRoute);
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<BusRoute, BusRouteDTO>(busRoute);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
