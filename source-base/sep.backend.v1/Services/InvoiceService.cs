using Microsoft.EntityFrameworkCore;
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
using sep.backend.v1.Services.UnitOfWorks;

namespace sep.backend.v1.Services;

public class InvoiceService : BaseService<InvoiceDTO, Invoice>, IInvoiceService
{
    private readonly IEmailInvoiceService _emailInvoiceService;
    private readonly IJobSendInvoice _jobSendInvoice;

    public InvoiceService(IUnitOfWork unitOfWork, IAutoMapper mapper, IEmailInvoiceService emailInvoiceService,
        IJobSendInvoice jobSendInvoice) : base(unitOfWork, mapper)
    {
        _emailInvoiceService = emailInvoiceService;
        _jobSendInvoice = jobSendInvoice;
    }

    public async Task<CreateInvoiceDTO> CreateInvoiceAsync(CreateInvoiceDTO model)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            await CheckExistSchoolAndSubscriptionAsync(model.SchoolId, model.SubscriptionPlanId);
            var subscription = await GetSubscriptionPlanAsync(model.SubscriptionPlanId);
            var newSchoolSubscription = await CreateSchoolSubscriptionAsync(model, subscription);
            var newInvoice = await CreateInvoiceAsync(model, newSchoolSubscription.Id);

            if (newInvoice.Status == (int)InvoiceStatuses.Paid)
                await CreatePaymentAsync(newInvoice.Id, subscription.Price, model.PaymentMethod);

            await SendInvoiceEmailAsync(model.SchoolId, newInvoice, subscription);
            await _unitOfWork.CommitTransactionAsync();

            return model;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw new Exception(e.Message);
        }
    }

    public async Task<InvoiceDTO> GetInvoiceByIdAsync(int invoiceId)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetInvoiceByIdAsync(invoiceId);
        if (invoice is null)
            throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

        return _mapper.Map<Invoice, InvoiceDTO>(invoice);
    }

    public async Task<PagedResponse<List<InvoiceDTO>>> GetAllInvoicesAsync(PaginationFilter filters,
        IUriService uriService,
        string route, int? status, string? keyword)
    {
        var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
        var pagedData = await _unitOfWork.InvoiceRepository.GetAllInvoicesAsync(status, keyword);
        var totalRecords = pagedData.Count();
        var invoiceDtos = _mapper.Map<List<Invoice>, List<InvoiceDTO>>(pagedData);

        var pagedResponse = PagedResponseHelper.CreatePagedResponse(
            invoiceDtos.AsQueryable(),
            validFilter,
            totalRecords,
            uriService,
            route
        );

        return pagedResponse;
    }

    public async Task<bool> UpdateInvoiceAsync(CreateInvoiceDTO model, int id)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetInvoiceByIdAsync(id);
        if (invoice is null)
            throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

        if (invoice.Status == (int)InvoiceStatuses.Paid)
            throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT, "Hóa đơn đã được thanh toán"));

        if (model.Status == (int)InvoiceStatuses.Paid)
            await CreatePaymentAsync(invoice.Id, invoice.SchoolSubscriptionPlans.SubscriptionPlan.Price,
                model.PaymentMethod);

        invoice.Status = model.Status;
        await _unitOfWork.InvoiceRepository.Update(invoice);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> DeleteMultipleInvoiceAsync(IEnumerable<int> invoiceIds)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var invoices = _unitOfWork.GetRepository<Invoice>()
                .Where(iv => invoiceIds.Contains(iv.Id))
                .Include(iv => iv.Payments) // Bao gồm các liên kết liên quan
                .ToList();

            if (!invoices.Any())
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

            var hasRelations = invoices.Any(iv => iv.Payments != null && iv.Payments.Any());
            if (hasRelations)
                throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT,
                    "Không thể xóa hóa đơn vì một số hóa đơn đã có liên kết thanh toán"));

            await _unitOfWork.GetRepository<Invoice>().RemoveRange(invoices);
            await _unitOfWork.CompleteAsync();
            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackTransactionAsync();
            Console.WriteLine(e.Message);
            throw;
        }
    }


    public async Task<bool> ScheduleInvoiceAsync()
    {
        var getListSchool = await _unitOfWork.SchoolRepository.getAllSchoolExpired();

        var emailScheduleDTOs = getListSchool.Select(school => new EmailScheduleDTO
        {
            SchoolId = school.Id,
            Email = school.Email
        }).ToList();

        await _jobSendInvoice.ScheduleInvoiceAsync(emailScheduleDTOs.ToArray());
        return true;
    }

    public Task<PaymentDTO> CreatePaymentAsync(PaymentDTO model)
    {
        var payment = _mapper.Map<PaymentDTO, Payment>(model);

        _unitOfWork.GetRepository<Payment>().Add(payment);
        _unitOfWork.CompleteAsync();

        return Task.FromResult(model);
    }

    private async Task<bool> CheckExistSchoolAndSubscriptionAsync(int schoolId, int subscriptionId)
    {
        var school = await _unitOfWork.GetRepository<School>().GetById(schoolId);
        if (school is null)
            throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Trường học"));

        var subscription = await _unitOfWork.GetRepository<SubscriptionPlan>().GetById(subscriptionId);
        if (subscription is null)
            throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói dịch vụ"));

        return true;
    }

    private async Task<SubscriptionPlan> GetSubscriptionPlanAsync(int subscriptionId)
    {
        return await _unitOfWork.GetRepository<SubscriptionPlan>().GetById(subscriptionId);
    }

    private async Task CreatePaymentAsync(int invoiceId, decimal amount, string paymentMethod)
    {
        var newPayment = new Payment
        {
            InvoiceId = invoiceId,
            Amount = amount,
            PaymentDate = DateTime.UtcNow,
            PaymentMethod = paymentMethod,
            Status = (int)Statuses.Active
        };

        await _unitOfWork.GetRepository<Payment>().Add(newPayment);
        await _unitOfWork.CompleteAsync();
    }

    private async Task SendInvoiceEmailAsync(int schoolId, Invoice newInvoice, SubscriptionPlan subscription)
    {
        var school = await _unitOfWork.GetRepository<School>().GetById(schoolId);
        await _emailInvoiceService.SendInvoiceEmailAsync(
            school.Email,
            newInvoice.Id.ToString(),
            newInvoice.IssueDate.ToString(),
            newInvoice.DueDate.ToString(),
            school.Name,
            subscription.Name,
            subscription.DurationDays.ToString(),
            subscription.Price.ToString()
        );
    }

    private async Task<SchoolSubscriptionPlan> CreateSchoolSubscriptionAsync(CreateInvoiceDTO model,
        SubscriptionPlan subscription)
    {
        var newSchoolSubscription = new SchoolSubscriptionPlan
        {
            SchoolId = model.SchoolId,
            SubscriptionPlanId = model.SubscriptionPlanId,
            StartDate = DateTime.UtcNow,
            EndDate = DateHelper.AddMonthsToDate(DateTime.UtcNow, subscription.DurationDays),
            Status = (int)Statuses.Inactive
        };
        await _unitOfWork.GetRepository<SchoolSubscriptionPlan>().Add(newSchoolSubscription);
        await _unitOfWork.CompleteAsync();

        return newSchoolSubscription;
    }

    private async Task<Invoice> CreateInvoiceAsync(CreateInvoiceDTO model, int schoolSubscriptionPlanId)
    {
        var newInvoice = _mapper.Map<CreateInvoiceDTO, Invoice>(model);
        newInvoice.SchoolSubscriptionPlanId = schoolSubscriptionPlanId;
        await _unitOfWork.GetRepository<Invoice>().Add(newInvoice);
        await _unitOfWork.CompleteAsync();

        return newInvoice;
    }
    public async Task<PagedResponse<List<InvoiceDTO>>> GetAllInvoiceOfSchoolAsync(
        PaginationFilter filters, 
        IUriService uriService, 
        string route, 
        int schoolId, 
        int? invoiceStatus = null, 
        DateTime? startDate = null,
        DateTime? endDate = null)
    {

        var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
        var invoices = await _unitOfWork.InvoiceRepository.GetInvoiceOfSchoolAsync(schoolId, invoiceStatus, startDate, endDate);

        var invoiceDTOs = invoices.Select(invoices => _mapper.Map<Invoice, InvoiceDTO>(invoices)).ToList();
        var totalRecords = invoices.Count();
        var pagedResponse = PagedResponseHelper.CreatePagedResponse(
            invoiceDTOs.AsQueryable(),
            validFilter,
            totalRecords,
            uriService,
            route
        );

        return pagedResponse;
    }
    public async Task<InvoiceDTO> GetNewestInvoice(int subscriptionPlanId)
    {
        var invoice = await _unitOfWork.InvoiceRepository.GetNewestInvoiceAsync(subscriptionPlanId);
        if (invoice is null)
            throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

        return _mapper.Map<Invoice, InvoiceDTO>(invoice);
    }
}