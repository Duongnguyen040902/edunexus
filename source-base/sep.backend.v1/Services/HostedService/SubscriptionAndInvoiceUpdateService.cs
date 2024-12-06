using sep.backend.v1.Common.Enums;
using sep.backend.v1.Data.Entities;
using sep.backend.v1.Helpers;
using sep.backend.v1.Services.UnitOfWork;

namespace sep.backend.v1.Services.HostedService
{
    public class SubscriptionAndInvoiceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SubscriptionAndInvoiceUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    await UpdateInvoiceStatuses(unitOfWork);
                    await RegisterNewSubscriptionPlans(unitOfWork);
                }
            
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }

        private async Task UpdateInvoiceStatuses(IUnitOfWork unitOfWork)
        {
            var invoices = await unitOfWork.GetRepository<Invoice>()
                .GetMulti(i => i.DueDate < DateTime.UtcNow && i.Status == (int)InvoiceStatuses.Pending);

            foreach (var invoice in invoices)
            {
                invoice.Status = (int)InvoiceStatuses.Cancel;
            }

            await unitOfWork.CompleteAsync();
        }

        private async Task RegisterNewSubscriptionPlans(IUnitOfWork unitOfWork)
        {  
            var currentPackages = await unitOfWork.GetRepository<SchoolSubscriptionPlan>()
                .GetMulti(p => p.Status == (int)Statuses.Active);

            foreach (var package in currentPackages)
            {
                if (package.EndDate <= DateTime.UtcNow)
                {
                    package.Status = (int)Statuses.Inactive;
                    await unitOfWork.CompleteAsync();

                    var subscriptionPlan = unitOfWork.GetRepository<SubscriptionPlan>().Where(s => s.Price == 0).FirstOrDefault();
                    if (subscriptionPlan != null)
                    {
                        var newSchoolSubscriptionPlan = new SchoolSubscriptionPlan
                        {
                            SchoolId = package.SchoolId,
                            SubscriptionPlanId = subscriptionPlan.Id,
                            Status = (int)Statuses.Active,
                            StartDate = DateTime.UtcNow,
                            EndDate = DateHelper.AddMonthsToDate(DateTime.UtcNow, subscriptionPlan.DurationDays),
                        };

                        await unitOfWork.GetRepository<SchoolSubscriptionPlan>().Add(newSchoolSubscriptionPlan);
                        await unitOfWork.CompleteAsync();
                    }
                }
            }
        }
    }
}

