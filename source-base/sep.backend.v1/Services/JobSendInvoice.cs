using sep.backend.v1.DTOs;
using sep.backend.v1.Services.IRepositories;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services;

public class JobSendInvoice : JobExecutorService, IJobSendInvoice
{
    private readonly ISchoolRepository _schoolRepository;
    public JobSendInvoice(IEmailService emailService, ISchoolRepository schoolRepository) : base(emailService)
    {
        _schoolRepository = schoolRepository;
    }

    protected override async Task ExecuteJobAsync()
    {
        var getListSchool = await _schoolRepository.getAllSchoolExpired();

        var emailScheduleDTOs = getListSchool.Select(school => new EmailScheduleDTO
        {
            SchoolId = school.Id,
            Email = school.Email
        }).ToArray();

        await ScheduleInvoiceAsync(emailScheduleDTOs);
    }

    public async Task ScheduleInvoiceAsync(EmailScheduleDTO[] emailScheduleDTOs)
    {
        var tasks = emailScheduleDTOs.Select(dto =>
            _emailService.SendEmailAsync(dto.Email, "Invoice Reminder", "This is your scheduled invoice reminder."));
        await Task.WhenAll(tasks);
    }
}