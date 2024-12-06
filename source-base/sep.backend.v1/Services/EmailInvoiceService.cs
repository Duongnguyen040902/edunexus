using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using sep.backend.v1.Services.IServices;

namespace sep.backend.v1.Services
{
    public class EmailInvoiceService : EmailService, IEmailInvoiceService
    {
        public EmailInvoiceService(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task SendInvoiceEmailAsync(
            string email, 
            string invoiceId, 
            string issueDate, 
            string dueDate, 
            string schoolName, 
            string subscriptionPlan, 
            string subscriptionPlanDate, 
            string price)
        {
            var body = Helpers.TemplateHelper.GetEmailTemplate("invoice.html");
            body = body.Replace("{{invoice_id}}", invoiceId)
                .Replace("{{issue_date}}", issueDate)
                .Replace("{{due_date}}", dueDate)
                .Replace("{{school_name}}", schoolName)
                .Replace("{{subscription_plan}}", subscriptionPlan)
                .Replace("{{subscription_plan_date}}", subscriptionPlanDate)
                .Replace("{{price}}", price);

            await SendEmailAsync(email, "Your Invoice", body);
        }

        public async Task PaymentSuccessEmailAsync(
            string email,
            string invoiceId,
            string paymentDate,
            string paymentMethod,
            string price,
            string subscriptionPlan
            )
        {
            var body = Helpers.TemplateHelper.GetEmailTemplate("payment.html");
            body = body.Replace("{{invoice_id}}", invoiceId)
                .Replace("{{payment_date}}", paymentDate)
                .Replace("{{payment_method}}", paymentMethod)
                .Replace("{{price}}", price)
                .Replace("{{subscription_plan}}", subscriptionPlan);

            await SendEmailAsync(email, "THÔNG TIN THANH TOÁN", body);
        }
    }
}