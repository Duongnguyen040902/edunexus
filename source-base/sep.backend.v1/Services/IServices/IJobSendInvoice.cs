using sep.backend.v1.DTOs;

namespace sep.backend.v1.Services.IServices;

public interface IJobSendInvoice : IJobExecutorService
{
    Task ScheduleInvoiceAsync(EmailScheduleDTO[] emailScheduleDTOs);
}