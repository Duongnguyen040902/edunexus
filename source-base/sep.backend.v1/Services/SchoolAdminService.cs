using sep.backend.v1.Common.Const;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services;

public class SchoolAdminService : BaseService<SchoolDTO, School>, ISchoolAdminService
{
    public SchoolAdminService(IUnitOfWork unitOfWork, IAutoMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<CreateSchoolDTO> CreateSchoolAdminAsync(CreateSchoolDTO schoolEntity)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var newSchool = await CreateNewSchoolAsync(schoolEntity);
            if (schoolEntity.SubscriptionPlanId.HasValue)
            {
                var newSchoolSubscriptionPlan = await AddSchoolSubscriptionPlanAsync(
                    newSchool.Id,
                    schoolEntity.SubscriptionPlanId.Value
                );

                if (schoolEntity.SubscriptionPlanId != (int)Subscription.Trial)
                {
                    await AddInvoiceAndPaymentAsync(newSchoolSubscriptionPlan, schoolEntity.PaymentMethod);
                }
            }

            await _unitOfWork.CommitTransactionAsync();
            return _mapper.Map<School, CreateSchoolDTO>(newSchool);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task<School> CreateNewSchoolAsync(CreateSchoolDTO schoolEntity)
    {
        schoolEntity.Role = ShortRoleName.SCHOOL_ADMIN;
        schoolEntity.AccountStatus = (int)Statuses.Active;
        var newSchool = _mapper.Map<CreateSchoolDTO, School>(schoolEntity);
        await _unitOfWork.SchoolRepository.Add(newSchool);
        await _unitOfWork.CompleteAsync();
        return newSchool;
    }

    private async Task<SchoolSubscriptionPlan> AddSchoolSubscriptionPlanAsync(int schoolId, int subscriptionPlanId)
    {
        var subscription = await _unitOfWork.SubscriptionPlanRepository.GetById(subscriptionPlanId);
        var newSchoolSubscriptionPlan = new SchoolSubscriptionPlan
        {
            SchoolId = schoolId,
            SubscriptionPlanId = subscriptionPlanId,
            Status = (int)Statuses.Active,
            StartDate = DateTime.UtcNow,
            EndDate = DateHelper.AddMonthsToDate(DateTime.UtcNow, subscription.DurationDays),
        };

        await _unitOfWork.SchoolSubscriptionRepository.Add(newSchoolSubscriptionPlan);
        await _unitOfWork.CompleteAsync();
        return newSchoolSubscriptionPlan;
    }

    private async Task AddInvoiceAndPaymentAsync(SchoolSubscriptionPlan newSchoolSubscriptionPlan, string paymentMethod)
    {
        var subscription =
            await _unitOfWork.SubscriptionPlanRepository.GetById(newSchoolSubscriptionPlan.SubscriptionPlanId);

        var newInvoice = new Invoice
        {
            SchoolSubscriptionPlanId = newSchoolSubscriptionPlan.Id,
            IssueDate = DateTime.UtcNow,
            DueDate = DateHelper.AddMonthsToDate(DateTime.UtcNow, subscription.DurationDays),
            Status = (int)InvoiceStatuses.Paid,
        };
        await _unitOfWork.InvoiceRepository.Add(newInvoice);
        await _unitOfWork.CompleteAsync();

        if (subscription.Id == (int)Subscription.Trial)
        {
            return;
        }

        var newPayment = new Payment
        {
            InvoiceId = newInvoice.Id,
            Amount = subscription.Price,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = paymentMethod,
            Status = (int)PaymentStatuses.Success,
        };
        await _unitOfWork.GetRepository<Payment>().Add(newPayment);
        await _unitOfWork.CompleteAsync();
    }

    public Task<SchoolDTO> DeleteSchoolAdminAsync(int schoolId)
    {
        throw new NotImplementedException(); //TODO: PENDING QA
    }

    public async Task<PagedResponse<List<SchoolDTO>>> GetAllAccountSchoolAdmin(PaginationFilter filters,
        IUriService uriService, string route, int? status, string? keyword, int? subscriptionPlanId)
    {
        var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
        var pagedData =
            await _unitOfWork.SchoolRepository.getAllAccountSchoolAdmin(status, keyword, subscriptionPlanId);
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

    public async Task<SchoolDTO> GetSchoolAdminAsync(int schoolId)
    {
        var accountSchool = await _unitOfWork.SchoolRepository.getAccountSchoolAdminBySchoolId(schoolId);
        if (accountSchool == null)
        {
            var placeholders = new Dictionary<string, string>
            {
                { "attribute", "người dùng" }
            };
            throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate, placeholders));
        }

        return _mapper.Map<School, SchoolDTO>(accountSchool);
    }

    public async Task<UpdateSchoolDTO> UpdateSchoolAdminAsync(int id, UpdateSchoolDTO schoolEntity)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var school = await _unitOfWork.SchoolRepository.GetById(id);
            if (school == null)
            {
                var placeholders = new Dictionary<string, string>
                {
                    { "attribute", "người dùng" }
                };
                throw new NotFoundException(StringHelper.FormatMessage(Responses.NotFoundMessageTemplate,
                    placeholders));
            }

            _mapper.Map(schoolEntity, school);
            _unitOfWork.SchoolRepository.Update(school);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<School, UpdateSchoolDTO>(school);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}