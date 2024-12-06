using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services;

public abstract class JobExecutorService : IJobExecutorService
{
    protected readonly IEmailService _emailService;

    protected JobExecutorService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task ExecuteAsync()
    {
        await ExecuteJobAsync();
    }

    protected abstract Task ExecuteJobAsync();
}