namespace sep.backend.v1.Services.IServices;

public interface IEmailInvoiceService
{
    Task SendInvoiceEmailAsync(string email,
        string invoiceId,
        string issueDate,
        string dueDate,
        string schoolName,
        string subscriptionPlan,
        string subscriptionPlanDate,
        string price);


    Task PaymentSuccessEmailAsync(
            string email,
            string invoiceId,
            string paymentDate,
            string paymentMethod,
            string price,
            string subscriptionPlan);
}