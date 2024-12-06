using Microsoft.EntityFrameworkCore;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using System.Linq.Expressions;
using sep.backend.v1.Common.Enums;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Services;

public class ClubService : BaseService<ClubDTO, Club>, IClubService
{
    public ClubService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<List<ClubDTO>> GetClubsByConditionAsync(Expression<Func<ClubEnrollment, bool>> predicate)
    {
        var clubEnrollments = await _unitOfWork.GetRepository<ClubEnrollment>()
            .GetMulti(predicate, new string[] { "Semester", "Club" });
        if (clubEnrollments.Count() == 0)
        {
            return null;
        }
        var clubs = clubEnrollments
            .OrderByDescending(x => x.Id)
            .Select(x => x.Club)
            .ToList();

        return _mapper.Map<List<Club>, List<ClubDTO>>(clubs);
    }

    public async Task<List<ClubDTO>> GetAssignClubAsync(int teacherId)
    {
        return await GetClubsByConditionAsync(x => x.Semester.IsActive && x.TeacherId == teacherId);
    }

    public async Task<ClubDetailDTO> GetClubAsync(int clubId, int semesterId)
    {
        var club = await _unitOfWork.ClubRepository.GetClubDetailAsync(clubId, semesterId);
        if (club == null)
        {
            throw new NotFoundException("Không có dữ liệu");
        }
        var result = _mapper.Map<Club, ClubDetailDTO>(club);

        return result;
    }

    public async Task<List<ClubDTO>> GetEnrolledClubAsync(int pupilId)
    {
        return await GetClubsByConditionAsync(x => x.Semester.IsActive && x.PupilId == pupilId);
    }

    public async Task<PagedResponse<List<ClubDetailDTO>>> GetClubsBySchoolId(PaginationFilter filters,
        IUriService uriService, string route, int schoolId)
    {
        var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
        var pagedData = await _unitOfWork.ClubRepository.GetClubsDetailAsync(schoolId);
        var totalRecords = pagedData.Count();
        var clubsDto = _mapper.Map<List<Club>, List<ClubDetailDTO>>(pagedData);
        var pagedResponse = PagedResponseHelper.CreatePagedResponse(
            clubsDto.AsQueryable(),
            validFilter,
            totalRecords,
            uriService,
            route
        );

        return pagedResponse;
    }

    public async Task<PagedResponse<List<ClubDTO>>> GetListClubs(PaginationFilter filters, IUriService uriService, string route, int? status, string? keyword, int? schoolId)
    {
        var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
        var pagedData = await _unitOfWork.ClubRepository.GetAllClubs(status, keyword, schoolId);
        var totalRecords = pagedData.Count();
        var clubDto = _mapper.Map<List<Club>, List<ClubDTO>>(pagedData);

        return PagedResponseHelper.CreatePagedResponse(clubDto.AsQueryable(), validFilter, totalRecords, uriService, route);
    }

    public async Task<ClubDTO> CreateClub(CreateClubDTO clubDto, int schoolId)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var existingClub = await _unitOfWork.ClubRepository.GetSingleByCondition(c => c.Name == clubDto.Name && c.SchoolId == schoolId);
            if (existingClub != null)
                throw new ConflictException(Responses.ConflictNameClub);

            var newClub = _mapper.Map<CreateClubDTO, Club>(clubDto);
            newClub.SchoolId = schoolId;

            await _unitOfWork.ClubRepository.Add(newClub);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<Club, ClubDTO>(newClub);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ClubDTO?> UpdateClub(int id, CreateClubDTO clubDto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var club = await _unitOfWork.ClubRepository.GetById(id);
            if (club == null)
            {
                var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "câu lạc bộ" }
                            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            var exisClub = await _unitOfWork.ClubRepository.GetSingleByCondition(x => x.Name == clubDto.Name && x.Id != id && x.SchoolId == club.SchoolId);
            if (exisClub != null)
                throw new NotFoundException(Responses.ConflictNameClub);
            _mapper.Map(clubDto, club);
            _unitOfWork.ClubRepository.Update(club);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<Club, ClubDTO>(club);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteClub(int id)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var club = await _unitOfWork.ClubRepository.GetById(id);
            if (club == null)
            {
                var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "câu lạc bộ" }
                            };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
            }
            _unitOfWork.ClubRepository.Delete(club.Id);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ClubDTO> GetClubDetail(int clubId)
    {
        var club = await _unitOfWork.ClubRepository.GetById(clubId);
        if (club == null)
        {
            var placeholders = new Dictionary<string, string>
                            {
                                { "attribute", "câu lạc bộ" }
                            };
            throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
        }
        var result = _mapper.Map<Club, ClubDTO>(club);

        return result;
    }
}
