using sep.backend.v1.Common.Filters;
using sep.backend.v1.Common.Responses;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.DTOs;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.IServices;
using sep.backend.v1.Services.UnitOfWork;
using sep.backend.v1.Common.Enums;
using sep.backend.v1.Exceptions;
using sep.backend.v1.Services.UnitOfWorks;
using sep.backend.v1.Common.Const;

namespace sep.backend.v1.Services
{
    public class SchoolSubscriptionService : ISchoolSubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAutoMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailInvoiceService _emailInvoiceService;

        public SchoolSubscriptionService(IUnitOfWork unitOfWork,
            IAutoMapper mapper,
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IEmailInvoiceService emailInvoiceService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
            _emailInvoiceService = emailInvoiceService;
        }

        public async Task<PagedResponse<List<SchoolSubscriptionDTO>>> GetAllSubscriptionsOfSchoolAsync(
            PaginationFilter filters,
            IUriService uriService,
            string route,
            int schoolId,
            int? status = null,
            int? year = null)
        {

            var validFilter = new PaginationFilter(filters.PageNumber, filters.PageSize);
            var subscriptions = await _unitOfWork.SchoolSubscriptionRepository.GetAllSubscriptionOfSchoolAsync(schoolId, status, year);
            if (subscriptions is null)
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Danh sách gói đăng ký"));

            var subscriptionDTOs = subscriptions.Select(subscriptions => _mapper.Map<SchoolSubscriptionPlan, SchoolSubscriptionDTO>(subscriptions)).ToList();

            var totalRecords = subscriptions.Count();

            var pagedResponse = PagedResponseHelper.CreatePagedResponse(
                subscriptionDTOs.AsQueryable(),
                validFilter,
                totalRecords,
                uriService,
                route
                );

            return pagedResponse;


        }

        public async Task<SchoolSubscriptionDTO?> GetCurrentSubscriptionOfSchoolAsync(int schoolId)
        {

            var subscription = await _unitOfWork.SchoolSubscriptionRepository.GetCurrentSubscriptionOfSchoolAsync(schoolId);

            if (subscription is null)
                throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói hiện tại"));

            var subscriptionDTO = _mapper.Map<SchoolSubscriptionPlan, SchoolSubscriptionDTO>(subscription);

            return subscriptionDTO;

        }
        
        public async Task<InvoiceDTO> CreateInvoiceForSubscriptionAsync(int schoolId, int subscriptionPlanId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var subscriptionPlan = await _unitOfWork.GetRepository<SubscriptionPlan>().GetById(subscriptionPlanId);
                if (subscriptionPlan is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Gói đăng ký"));

                var newSchoolSubscriptionPlan = await CreateSchoolSubscriptionAsync(schoolId, subscriptionPlan, (int)Statuses.Inactive);     

                var invoice = await CreateInvoiceAsync(newSchoolSubscriptionPlan.Id, DateTime.UtcNow, (int)InvoiceStatuses.Pending);

                await _unitOfWork.CommitTransactionAsync();
                var newInvoice = await _unitOfWork.InvoiceRepository.GetInvoiceDetailAsync(invoice.Id);
                var invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(newInvoice);

                return invoiceDTO;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<string> GeneratePaymentUrlAsync(int invoiceId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var invoice = await _unitOfWork.GetRepository<Invoice>().GetById(invoiceId);
                if (invoice is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

                if (invoice.Status != (int)InvoiceStatuses.Pending)
                    throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT, "Hóa đơn đã được thanh toán hoặc bị hủy"));

                var schoolSubscriptionPlan = await _unitOfWork.SchoolSubscriptionRepository.GetSubscriptionByInvoiceIdAsync(invoiceId); 
                if (schoolSubscriptionPlan is null)
                    throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Thông tin đăng ký cho hóa đơn này"));

                if (schoolSubscriptionPlan.SubscriptionPlan.Price <= 0)
                    throw new InvalidOperationException("Giá trị gói đăng ký không hợp lệ");

                var pendingInvoices = await _unitOfWork.GetRepository<Invoice>()
                    .GetMulti(i => i.Status == (int)InvoiceStatuses.Pending && i.Id != invoiceId);

                foreach (var pendingInvoice in pendingInvoices)
                {
                    pendingInvoice.Status = (int)InvoiceStatuses.Cancel;
                }
                await _unitOfWork.CompleteAsync();               

                var vnPayRequest = new VnPaymentRequestDTO
                {
                    OrderId = new Random().Next(1000, 100000),
                    SubscriptionPlan = schoolSubscriptionPlan.SubscriptionPlan.Name,
                    InvoiceId = invoiceId,
                    Amount = (double)schoolSubscriptionPlan.SubscriptionPlan.Price,
                    CreatedDate = DateTime.UtcNow
                };

                var vnPayService = new VnPayService(_config, _httpContextAccessor);
                var paymentUrl = vnPayService.CreatePaymentUrl(_httpContextAccessor.HttpContext, vnPayRequest);

                await _unitOfWork.CommitTransactionAsync();
                return paymentUrl;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }


        public async Task<bool> ProcessPaymentCallbackAsync(HttpContext httpContext)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var vnPayService = new VnPayService(_config, _httpContextAccessor);
                var paymentResponse = await vnPayService.PaymentExecute(httpContext.Request.Query);
                var invoiceId = int.TryParse(paymentResponse?.InvoiceId, out var id) ? id : 0;

                var invoice = await _unitOfWork.GetRepository<Invoice>().GetById(invoiceId)
                              ?? throw new NotFoundException(StringHelper.FormatMessage(Messages.NOT_FOUND, "Hóa đơn"));

                if (invoice.Status != (int)InvoiceStatuses.Pending )
                    throw new ConflictException(StringHelper.FormatMessage(Messages.CONFLICT, "Hóa đơn đã được thanh toán hoặc bị hủy"));

                var schoolSubscriptionPlan = await _unitOfWork.SchoolSubscriptionRepository.GetSubscriptionByInvoiceIdAsync(invoiceId);

                bool isSuccess = paymentResponse?.TransactionStatus == "00" && paymentResponse.VnPayResponseCode == "00";

                var paymentStatus = isSuccess ? PaymentStatuses.Success : PaymentStatuses.Error;
                var invoiceStatus = isSuccess ? (int)InvoiceStatuses.Paid : invoice.Status;

                var payment = new Payment
                {
                    InvoiceId = invoiceId,
                    Amount = schoolSubscriptionPlan.SubscriptionPlan?.Price ?? 0,
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = "VnPay",
                    Status = (int)paymentStatus
                };

                await _unitOfWork.GetRepository<Payment>().Add(payment);

                if (isSuccess)
                {
                    var activeSubscriptions = await _unitOfWork
                    .GetRepository<SchoolSubscriptionPlan>()
                    .GetMulti(predicate: ssp => ssp.SchoolId == schoolSubscriptionPlan.SchoolId && ssp.Status == (int)Statuses.Active);

                    foreach (var subscription in activeSubscriptions)
                    {
                        subscription.Status = (int)Statuses.Inactive;
                    }

                    invoice.Status = (int)InvoiceStatuses.Paid;
                    schoolSubscriptionPlan.Status = (int)Statuses.Active;

                    await SendInvoiceEmailAsync(schoolSubscriptionPlan.School.Email, invoice, schoolSubscriptionPlan.SubscriptionPlan);
                }
                
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();
                return isSuccess;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        

        private async Task<SchoolSubscriptionPlan> CreateSchoolSubscriptionAsync(int schoolId, SubscriptionPlan subscriptionPlan, int status)
        {
            var newSchoolSubscription = new SchoolSubscriptionPlan
            {
                SchoolId = schoolId,
                SubscriptionPlanId = subscriptionPlan.Id,
                Status = status,
                StartDate = DateTime.UtcNow,
                EndDate = DateHelper.AddMonthsToDate(DateTime.UtcNow, subscriptionPlan.DurationDays),

            };
            await _unitOfWork.GetRepository<SchoolSubscriptionPlan>().Add(newSchoolSubscription);
            await _unitOfWork.CompleteAsync();

            return newSchoolSubscription;
        }
        private async Task<Invoice> CreateInvoiceAsync(int schoolSubscriptionPlanId, DateTime issueDate, int status)
        {
            var invoice = new Invoice
            {
                SchoolSubscriptionPlanId = schoolSubscriptionPlanId,
                IssueDate = issueDate,
                DueDate = issueDate.AddDays(1),
                Status = status,
            };

            await _unitOfWork.GetRepository<Invoice>().Add(invoice);
            await _unitOfWork.CompleteAsync();

            return invoice;
        }

        private async Task SendInvoiceEmailAsync(string email, Invoice newInvoice, SubscriptionPlan subscription)
        {
            await _emailInvoiceService.PaymentSuccessEmailAsync(
                email,
                newInvoice.Id.ToString(),                   
                DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss"), 
                "VnPay",                                   
                subscription.Price.ToString("N0"),            
                subscription.Name                 
            );
        }



    }
}
